using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour {
    public Transform a;
    public Transform z;
    public float period;
    public float phase;

	void Update () {
        Vector3 center = (a.transform.position + z.transform.position) / 2;
        Vector3 radius = (a.transform.position - z.transform.position) / 2;
        transform.position = center + radius * Mathf.Sin((Time.time + phase) / period * 2 * Mathf.PI);
	}
}
