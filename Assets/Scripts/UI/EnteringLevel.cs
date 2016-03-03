using UnityEngine;
using System.Collections;

public class EnteringLevel : MonoBehaviour {
    void Update() {
        if (Input.anyKey) {
            gameObject.SetActive(false);
            PauseManager.paused = false;
        }
    }
}
