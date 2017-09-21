using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;

    List<float> times = new List<float>();

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        if (times.Count > 1000) {
            times.Clear();
        }
        times.Add(msec);
        float maxtime = times.Max();
        string text = string.Format("{0:0.0} ms ({1:0.} fps) [max {2:0.0} ms]", msec, fps, maxtime);
        GUI.Label(rect, text, style);
    }
}