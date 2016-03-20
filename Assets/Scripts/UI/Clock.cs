using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Clock : MonoBehaviour {
    const float maxTime = 30;

    Text text;
    List<LiftLowerWaitRaise> lifts;

    void Awake() {
        text = GetComponent<Text>();
        lifts = FindObjectsOfType<LiftLowerWaitRaise>().ToList();
    }

    void Update() {
        float closestTimeToGo = lifts.ExtMin(lift => lift.TimeToGo());
        if (closestTimeToGo < maxTime) {
            text.text = (Mathf.Ceil(closestTimeToGo)).ToString();
        } else {
            text.text = "";
        }
    }
}
