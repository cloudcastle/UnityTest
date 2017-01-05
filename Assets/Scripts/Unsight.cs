using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Unsight : MonoBehaviour {
    public List<Eye> eyes = new List<Eye>();
    public float lastAcceptableTrackedTime;

    void Start() {
        //TimeManager.instance.beforeTrack += BeforeTrack;
        eyes = FindObjectsOfType<Eye>().ToList();
    }

    void CheckEyes() {
        return;
        for (int i = 0; i < 100; i++) {
            var badEyes = eyes.Where(eye => eye.GetUnderSight() == gameObject).ToList();
            if (badEyes.Any()) {
                TimeManager.instance.UndoToTime(TimeManager.GameTime-5);
                badEyes.ForEach(eye => {
                    var rigidBody = eye.unit.GetComponent<Rigidbody>();
                    if (rigidBody != null) {
                        rigidBody.velocity = Vector3.zero;
                        rigidBody.angularVelocity = Vector3.zero;
                    }
                });
                if (eyes.Any(eye => eye.GetUnderSight() == gameObject)) {
                    Debug.LogFormat("Still bad");
                } else {
                    break;
                }
            } else {
                lastAcceptableTrackedTime = TimeManager.GameTime;
                break;
            }
        }
    }

    //void BeforeTrack() {
    //    //CheckEyes();
    //}

    void Update() {
        CheckEyes();
    }

    //public void OnEye(Unit unit) {
    //    GameLose.instance.Lose();
    //}
}
