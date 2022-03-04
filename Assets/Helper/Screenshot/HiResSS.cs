using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[ExecuteInEditMode]
public class HiResSS : SerializedMonoBehaviour
{
    public Camera _Camera;
    static int _Numeral;

    public int Width;
    public int Height;

    bool IsRecording = false;
    public float EveryXSeconds = 0.2f;
    float ElapsedFrames = 0;
    int InnerElapsedFrames = 0;
    public int NumberOfFrames = 60;

    public string Name = "";

    public List<GameObject> RecordingGos = new List<GameObject>();

    public static string ScreenShotName(string Name, int width, int height)
    {
        return string.Format("{0}/screenshots/"+Name+"_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             _Numeral.ToString("000"));
    }

    [Button]
    public void Record()
    {
        _Numeral = 0;
        RecordingGos.ForEach((x) => { x.gameObject.SetActive(false); });
        RecordingGos.ForEach((x) => { x.gameObject.SetActive(true); });
        Time.timeScale = 0.25f;
        ElapsedFrames = EveryXSeconds;
        InnerElapsedFrames = 0;
        IsRecording = true;
    }


    void LateUpdate()
    {
        if(!IsRecording)
        {
            return;
        }
        ElapsedFrames += Time.deltaTime;
        if (ElapsedFrames >= EveryXSeconds)
        {
            InnerElapsedFrames++;
            ElapsedFrames = 0;
            RenderTexture rt = new RenderTexture(Width, Height, 24);
            _Camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(Width, Height, TextureFormat.ARGB32, true);
            _Camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, Width, Height), 0, 0);
            _Camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            DestroyImmediate(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(Name, Width, Height);
            System.IO.File.WriteAllBytes(filename, bytes);
            //Debug.Log(string.Format("Took screenshot to: {0}", filename));

            _Numeral++;
        }
        if(InnerElapsedFrames >= NumberOfFrames)
        {
            Time.timeScale = 1.0f;
            IsRecording = false;
        }
    }
}