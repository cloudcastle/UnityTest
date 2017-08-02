using UnityEngine;
using System.Collections;
using RSG;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Audio;

public class PitchByValue : MonoBehaviour
{
    public AudioSource sound;

    public float baseValue = 1;

    public void ChangePitch(Vector3 value) {
        sound.pitch = (TimeManager.instance.Undoing() ? -1 : 1) * value.magnitude / baseValue;
    }

    public void ChangePitch(float value) {
        sound.pitch = (TimeManager.instance.Undoing() ? -1 : 1) * value / baseValue;
    }
}