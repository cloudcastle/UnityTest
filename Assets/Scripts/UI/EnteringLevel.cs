using UnityEngine;
using System.Collections;

public class EnteringLevel : MonoBehaviour {
    void Update() {
        if (Input.anyKey) {
            gameObject.SetActive(false);
            TimeManager.paused = false;
        }
    }

    void Awake() {
        TimeManager.paused = true;
    }
}
