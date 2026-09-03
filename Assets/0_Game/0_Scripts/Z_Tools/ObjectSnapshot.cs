using System.IO;
using UnityEngine;

public class ObjectSnapshot : MonoBehaviour {
    public Camera snapshotCamera; // Camera pointed strictly at your GameObject
    public int resWidth = 512;
    public int resHeight = 512;

    [ContextMenu("Take Photo")]
    public void TakeGameObjectPhoto() {
        // 1. Create a temporary RenderTexture
        if(snapshotCamera == null) {
            snapshotCamera = Camera.main;   
        }
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        snapshotCamera.targetTexture = rt;

        // 2. Force the camera to render
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGBA32, false);
        snapshotCamera.Render();

        // 3. Read the pixels from the RenderTexture
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        // 4. Clean up the camera connection
        snapshotCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);

        // 5. Convert to PNG and save to your project folder
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = Path.Combine(Application.dataPath, $"GameObject_Photo_{System.DateTime.Now:yyyyMMdd_HHmmss}.png");
        File.WriteAllBytes(filename, bytes);

        Debug.Log($"Photo saved to: {filename}");
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh(); // Makes the image instantly appear in your Project window
#endif
    }
}