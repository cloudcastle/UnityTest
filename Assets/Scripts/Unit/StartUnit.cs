using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class StartUnit : MonoBehaviour
{
    public Unit unit;

    void Update() {
        if (Extensions.Editor()) {
            if (FindObjectsOfType<StartUnit>().ToList().Count != 1) {
                Debug.LogError("Too many StartUnit on scene!");
            }
        }
    }
}