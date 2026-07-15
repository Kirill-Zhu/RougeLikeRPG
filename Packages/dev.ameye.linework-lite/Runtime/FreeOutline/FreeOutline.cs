using System;
using System.Linq;
using LineworkLite.Common.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace LineworkLite.FreeOutline {
    [ExcludeFromPreset]
    [DisallowMultipleRendererFeature("Free Outline")]
    [Tooltip("Free Outline renders outlines by rendering an extruded version of an object behind the original object.")]
    [HelpURL("https://ameye.dev")]
    public class FreeOutline : ScriptableRendererFeature {
        [SerializeField] public FreeOutlineSettings settings;

        [SerializeField, HideInInspector] private Material maskMaterial;
        [SerializeField, HideInInspector] private Material outlineMaterial;
        [SerializeField, HideInInspector] private Material clearMaterial;

        private FreeOutlinePass m_OutlinePass;

        public override void Create() {
            m_OutlinePass = new FreeOutlinePass();
            if (settings == null) {
                settings = ScriptableObject.CreateInstance<FreeOutlineSettings>();
            }
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
            if (settings == null || m_OutlinePass == null) return;

            if (m_OutlinePass.Setup(ref settings, ref maskMaterial, ref outlineMaterial, ref clearMaterial)) {
                renderer.EnqueuePass(m_OutlinePass);
            }
        }

        private class FreeOutlinePass : ScriptableRenderPass {
            private FreeOutlineSettings settings;
            private Material mask, outlineBase, clear;
            private readonly ProfilingSampler maskSampler, outlineSampler;

            public FreeOutlinePass() {
                profilingSampler = new ProfilingSampler(nameof(FreeOutlinePass));
                maskSampler = new ProfilingSampler("FreeOutline.Mask");
                outlineSampler = new ProfilingSampler("FreeOutline.Outline");
            }

            public bool Setup(ref FreeOutlineSettings freeOutlineSettings, ref Material maskMaterial, ref Material outlineMaterial, ref Material clearMaterial) {
                settings = freeOutlineSettings;
                mask = maskMaterial;
                outlineBase = outlineMaterial;
                clear = clearMaterial;
                renderPassEvent = (RenderPassEvent)freeOutlineSettings.InjectionPoint;

                if (mask == null || outlineBase == null || settings == null || settings.Outlines == null) return false;

                foreach (var outline in settings.Outlines) {
                    if (outline == null) continue;
                    if (outline.material == null) {
                        outline.AssignMaterials(outlineBase);
                    }
                }

                foreach (var outline in settings.Outlines) {
                    if (outline == null || !outline.IsActive()) continue;

                    var material = outline.material;
                    if (material == null) continue;

                    var (srcBlend, dstBlend) = RenderUtils.GetSrcDstBlend(outline.blendMode);
                    material.SetInt(CommonShaderPropertyId.BlendModeSource, srcBlend);
                    material.SetInt(CommonShaderPropertyId.BlendModeDestination, dstBlend);

                    switch (outline.maskingStrategy) {
                        case MaskingStrategy.Stencil:
                            material.SetFloat(CommonShaderPropertyId.CullMode, (float)CullMode.Off);
                            break;
                        case MaskingStrategy.CullFrontFaces:
                            material.SetFloat(CommonShaderPropertyId.CullMode, (float)CullMode.Front);
                            break;
                    }

                    material.SetColor(CommonShaderPropertyId.OutlineColor, outline.color);
                    material.SetColor(ShaderPropertyId.OutlineOccludedColor, outline.occlusion == Occlusion.WhenOccluded ? outline.color : outline.occludedColor);
                    material.SetFloat(ShaderPropertyId.OutlineWidth, outline.width);

                    if (outline.scaleWithResolution) material.EnableKeyword(ShaderFeature.ScaleWithResolution);
                    else material.DisableKeyword(ShaderFeature.ScaleWithResolution);

                    switch (outline.referenceResolution) {
                        case Resolution._480: material.SetFloat(ShaderPropertyId.ReferenceResolution, 480.0f); break;
                        case Resolution._720: material.SetFloat(ShaderPropertyId.ReferenceResolution, 720.0f); break;
                        case Resolution._1080: material.SetFloat(ShaderPropertyId.ReferenceResolution, 1080.0f); break;
                        case Resolution.Custom: material.SetFloat(ShaderPropertyId.ReferenceResolution, outline.customResolution); break;
                    }

                    if (outline.extrusionMethod == ExtrusionMethod.ClipSpaceNormalVector) {
                        material.SetFloat(ShaderPropertyId.OutlineWidth, outline.width);
                        material.SetFloat(ShaderPropertyId.MinOutlineWidth, outline.minWidth);
                    } else {
                        material.SetFloat(ShaderPropertyId.OutlineWidth, outline.width * 0.015f);
                        material.SetFloat(ShaderPropertyId.MinOutlineWidth, outline.minWidth * 0.015f);
                    }

                    if (outline.enableOcclusion) material.EnableKeyword(ShaderFeature.Occlusion);
                    else material.DisableKeyword(ShaderFeature.Occlusion);
                    if (outline.scaling == Scaling.ScaleWithDistance) material.EnableKeyword(ShaderFeature.ScaleWithDistance);
                    else material.DisableKeyword(ShaderFeature.ScaleWithDistance);

                    switch (outline.occlusion) {
                        case Occlusion.Always: material.SetFloat(CommonShaderPropertyId.ZTest, (float)CompareFunction.Always); break;
                        case Occlusion.WhenOccluded: material.SetFloat(CommonShaderPropertyId.ZTest, (float)CompareFunction.GreaterEqual); break;
                        case Occlusion.WhenNotOccluded: material.SetFloat(CommonShaderPropertyId.ZTest, (float)CompareFunction.LessEqual); break;
                    }
                }

                return settings.Outlines.Any(ShouldRenderOutline);
            }

            private static bool ShouldRenderStencilMask(Outline outline) {
                return outline != null && outline.IsActive() && (outline.maskingStrategy == MaskingStrategy.Stencil || outline.occlusion != Occlusion.WhenNotOccluded);
            }

            private static bool ShouldRenderOutline(Outline outline) {
                return outline != null && outline.IsActive();
            }

            // ПЕРЕПИСАННЫЙ КЛАССИЧЕСКИЙ МЕТОД EXECUTE
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
                if (settings == null || settings.Outlines == null) return;

                CommandBuffer cmd = CommandBufferPool.Get();

                // Используем классический тег инициализации
                ShaderTagId mainTag = new ShaderTagId("UniversalForward");

                using (new ProfilingScope(cmd, profilingSampler)) {
                    // Очищаем буфер трафарета для маски
                    cmd.ClearRenderTarget(false, true, Color.clear);
                    context.ExecuteCommandBuffer(cmd);
                    cmd.Clear();

                    // 1. Отрисовка маски (Mask)
                    using (new ProfilingScope(cmd, maskSampler)) {
                        foreach (var outline in settings.Outlines) {
                            if (!ShouldRenderStencilMask(outline)) continue;

                            var drawingSettings = CreateDrawingSettings(mainTag, ref renderingData, SortingCriteria.CommonOpaque);
                            // Добавляем альтернативные проходы рендеринга вручную
                            drawingSettings.SetShaderPassName(1, new ShaderTagId("UniversalForwardOnly"));
                            drawingSettings.SetShaderPassName(2, new ShaderTagId("SRPDefaultUnlit"));

                            drawingSettings.overrideMaterial = mask;
                            drawingSettings.overrideMaterialPassIndex = 0;

                            var filteringSettings = new FilteringSettings(RenderQueueRange.opaque, outline.layerMask);
                            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);
                        }
                    }

                    // 2. Отрисовка самой обводки (Outline)
                    using (new ProfilingScope(cmd, outlineSampler)) {
                        foreach (var outline in settings.Outlines) {
                            if (!ShouldRenderOutline(outline)) continue;

                            var drawingSettings = CreateDrawingSettings(mainTag, ref renderingData, SortingCriteria.CommonOpaque);
                            drawingSettings.SetShaderPassName(1, new ShaderTagId("UniversalForwardOnly"));
                            drawingSettings.SetShaderPassName(2, new ShaderTagId("SRPDefaultUnlit")); drawingSettings.overrideMaterial = outline.material; drawingSettings.overrideMaterialPassIndex = 0; var filteringSettings = new FilteringSettings(RenderQueueRange.opaque, outline.layerMask); context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);
                        }
                    }
                }
                context.ExecuteCommandBuffer(cmd); CommandBufferPool.Release(cmd);
            }
        }
    }
}