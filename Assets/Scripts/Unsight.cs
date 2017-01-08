using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Unsight : MonoBehaviour, IUndo, IRewind {
    public List<Eye> eyes = new List<Eye>();
    public float lastAcceptableTrackedTime;

    void Start() {
        eyes = FindObjectsOfType<Eye>().ToList();
        lastAcceptableTrackedTime = float.PositiveInfinity;
        TimeManager.instance.undos.Add(this);
        TimeManager.instance.rewinds.Add(this);
    }

    void CheckEyes() {
        if (eyes.Where(eye => eye.GetUnderSight() == gameObject).Any()) {
            lastAcceptableTrackedTime = TimeManager.GameTime - 5;
        } 
    }

    public bool Undoing() {
        if (TimeManager.GameTime < lastAcceptableTrackedTime || TimeManager.GameTime == 0) {
            lastAcceptableTrackedTime = float.PositiveInfinity;
        }
        return lastAcceptableTrackedTime < float.PositiveInfinity;
    }

    public float Rewinding() {
        return lastAcceptableTrackedTime < float.PositiveInfinity ? 5 : 1;
    }

    void Update() {
        CheckEyes();
    }
}
