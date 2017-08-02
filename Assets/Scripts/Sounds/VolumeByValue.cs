using UnityEngine;
using System.Collections;
using RSG;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Audio;

public class VolumeByValue : MonoBehaviour
{
    public AudioSource sound;

    public float baseValue;

    public void Play(Vector3 value) {
        sound.PlayOneShot(sound.clip, value.magnitude / baseValue);
    }

    public void Play(float value) {
        sound.PlayOneShot(sound.clip, value / baseValue);
    }
}