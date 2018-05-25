using UnityEngine;
using System.Collections;

public class Screenshot : MonoBehaviour
{
    public Camera CaptureCamera;

    void Start()
    {
        if (CaptureCamera.Equals(null))
        {
            CaptureCamera = Camera.main;
        }
    }

    public Texture2D ScreenshotCaptureCamera()
    {
        return ScreenshotCaptureCamera(new Rect(0, 0, Screen.width, Screen.height));
    }

    public Texture2D ScreenshotCaptureCamera(Rect rect)
    {
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        CaptureCamera.targetTexture = rt;
        CaptureCamera.Render();
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        CaptureCamera.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(rt);
        return screenShot;
    }

}
