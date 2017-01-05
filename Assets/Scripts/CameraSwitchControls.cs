using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraSwitchControls : MonoBehaviour {
    public List<SwitchableCameraPlace> cameraPlaces;
    public SwitchableCameraPlace current;

    void Awake() {
        cameraPlaces = FindObjectsOfType<SwitchableCameraPlace>().ToList();
    }

    void Start() {
        current.Apply();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            current = cameraPlaces.CyclicNext(current);
            current.Apply();
        }
    }
}
