using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverOfRT: MonoBehaviour
{
    public RenderTexture PassedFromOther;


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(PassedFromOther, source);
        Graphics.Blit(source, destination);
        // Whatever other blits you may need
        RenderTexture.ReleaseTemporary(PassedFromOther);
    }
}