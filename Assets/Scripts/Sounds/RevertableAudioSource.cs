using UnityEngine;
using System.Collections;

public class RevertableAudioSource : MonoBehaviour
{
    AudioSource audioSource;

    public void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play() {
        if (audioSource.pitch < 0) {
            audioSource.pitch = 1;
        }
        audioSource.Play();
        audioSource.time = 1f;
        audioSource.pitch = -3;
    }
}