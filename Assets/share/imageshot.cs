using UnityEngine;
using System.Collections;

public class imageshot : MonoBehaviour
{
    public RenderTexture renderTexture;
    void OnEnable()
    {
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture((int)GetComponent<Camera>().pixelWidth, (int)GetComponent<Camera>().pixelHeight, 0);
            renderTexture.Create();
        }
        else
        {
            renderTexture.Release();
            renderTexture.width = (int)GetComponent<Camera>().pixelWidth;
            renderTexture.height = (int)GetComponent<Camera>().pixelHeight;
            renderTexture.Create();
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, renderTexture);
        enabled = false;
    }
}
