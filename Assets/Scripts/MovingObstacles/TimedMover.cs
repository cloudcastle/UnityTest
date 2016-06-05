using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public abstract class TimedMover : MonoBehaviour {
    public Predictor predictor;

    void Awake() {
        predictor = GetComponent<Predictor>();
    }

    void FixedUpdate() {
        transform.position = PositionByTime(TimeManager.StoppableGameTime);
        if (predictor != null) {
            predictor.Predict(PositionByTime(TimeManager.StoppableGameTime + TimeManager.StoppableFixedDeltaTime), transform.rotation.eulerAngles);
        }
	}

    void Update() {
        if (Extensions.Editor()) {
            transform.position = PositionByTime(TimeManager.StoppableGameTime); 
        }
    }

    protected abstract Vector3 PositionByTime(float time);
}
