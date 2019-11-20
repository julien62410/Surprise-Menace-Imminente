using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyShader : MonoBehaviour
{

    public Material spookyMat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, null, spookyMat);
    }
}
