using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnteringScreen : UIScreen {
    public float delay = 1f;
    public float disappearingTime = 1f;
    CanvasGroup group;

    void Awake() {
        group = GetComponent<CanvasGroup>();
    }

    void Update() {
        if (delay > 0) {
            delay -= Time.deltaTime;
        } else {
            group.alpha -= Time.deltaTime / disappearingTime;
        }
    }
}
