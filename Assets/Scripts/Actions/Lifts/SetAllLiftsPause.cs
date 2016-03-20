using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class SetAllLiftsPause : MonoBehaviour
{
    public float pause = 1;

    void Start() {
        FindObjectsOfType<LiftLowerWaitRaise>().ToList().ForEach(lift => lift.pause = pause);
    }

}