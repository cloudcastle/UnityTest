using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : MonoBehaviour {
    public AudioSource pickSound;

    public void Pick(Pickable p) {
        pickSound.Play();
    }
}
