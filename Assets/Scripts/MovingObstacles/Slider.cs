using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour {
    public Transform a;
    public Transform z;
    public float period;
    public float phase;

    public Predictor predictor;

    void Awake() {
        predictor = GetComponent<Predictor>();
    }

	void FixedUpdate() {
        transform.position = PositionByTime(TimeManager.GameTime);
        if (predictor != null) {
            predictor.Predict(PositionByTime(TimeManager.GameTime + TimeManager.GameFixedDeltaTime), transform.rotation.eulerAngles);
        }
	}

    Vector3 PositionByTime(float time) {
        Vector3 center = (a.transform.position + z.transform.position) / 2;
        Vector3 radius = (a.transform.position - z.transform.position) / 2; 
        return center + radius * Mathf.Sin((time + phase) / period * 2 * Mathf.PI);
    }
}
