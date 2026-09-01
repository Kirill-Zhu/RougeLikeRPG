using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyStrategy))]
public class EnemyStrategyEditor : Editor {
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height) {
        EnemyStrategy itemData = (EnemyStrategy)target;

        // Safety check to ensure target and sprite strategy icons exist
        if (itemData == null || itemData.Icon == null) {
            return base.RenderStaticPreview(assetPath, subAssets, width, height);
        }

        Sprite sprite = itemData.Icon;

        // Use Unity's dedicated AssetPreview API to safely fetch the thumbnail.
        // This handles compression, cropping, and prevents Worker9 texture format exceptions.
        Texture2D spritePreview = AssetPreview.GetAssetPreview(sprite);

        if (spritePreview != null) {
            // Create a clean, uncompressed target texture matching requested dimensions
            Texture2D previewTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

            // Safe copy that doesn't violate compression rules
            Graphics.CopyTexture(spritePreview, 0, 0, 0, 0, width, height, previewTexture, 0, 0, 0, 0);

            return previewTexture;
        }

        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }
}