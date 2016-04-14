using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnteringScreen : UIScreen {
    float delay = 0.5f;
    const float disappearingTime = 0.25f;
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
