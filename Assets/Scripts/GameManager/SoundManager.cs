using UnityEngine;
using System.Collections;
using RSG;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    const string GLOBAL_PITCH = "Global Pitch";
    const string MUSIC_PITCH = "Music Pitch";
    const string SOUND_PITCH = "Sound Pitch";

    public static SoundManager instance;

    public AudioMixer mixer;

    public List<AudioSource> pitchableAudioSources;

    public TimedFloat pitch = new TimedFloat(1, 0);

    public void Awake() {
        instance = this;
        pitchableAudioSources = FindObjectsOfType<PitchableAudioSource>().Select(pas => pas.GetComponent<AudioSource>()).ToList();
    }

    public void Start() {
        pitch = new TimedFloat(1, Time.realtimeSinceStartup);
    }

    public void LogLerpPitch() {
        float newPitch = Mathf.Clamp(0.25f, Mathf.Pow(TimeManager.StoppableTimeScale, 0.25f), 100f);
        float pitchLogSpeed = 2;
        float logPitch = Mathf.Log(pitch.value);
        float logNewPitch = Mathf.Log(newPitch);
        float logDelta = logNewPitch - logPitch;
        float maxLogDelta = (Time.realtimeSinceStartup - pitch.time) * pitchLogSpeed;
        if (pitch.value == 0 || newPitch == 0) {
            maxLogDelta *= 1e9f;
        }
        if (Mathf.Abs(logDelta) > maxLogDelta) {
            logDelta = Mathf.Sign(logDelta) * maxLogDelta;
        }
        float logNextPitch = logPitch + logDelta;
        pitch.value = Mathf.Exp(logNextPitch);
    }

    public void UpdatePitch() {
        LogLerpPitch();

        pitch.time = Time.realtimeSinceStartup;
        mixer.SetFloat(GLOBAL_PITCH, pitch.value);
        mixer.SetFloat(SOUND_PITCH, pitch.value);
        if (TimeManager.instance.Undoing()) {
            pitchableAudioSources.ForEach(pas => pas.pitch = -1);
        } else {
            pitchableAudioSources.ForEach(pas => pas.pitch = 1);
        }
    }

    public void Update() {
        UpdatePitch();
    }
}