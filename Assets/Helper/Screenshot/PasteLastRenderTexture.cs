using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasteLastRenderTexture: MonoBehaviour
{
    public Camera _Cam;
    RenderTexture myRenderTexture;
    public ReceiverOfRT _End;
    void OnPreRender()
    {
        Debug.Log("PRE RENDER:" + _Cam.name);
        myRenderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 24);
        _Cam.targetTexture = myRenderTexture;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Debug.Log("ON RENDER:" + _Cam.name);
    }

    void OnPostRender()
    {
        _End.PassedFromOther = myRenderTexture;
        _Cam.targetTexture = null;
      
    }
}