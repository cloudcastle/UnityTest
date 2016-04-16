using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartUnit : MonoBehaviour
{
    void Start() {
        Player.instance.Control(GetComponent<Unit>());
    }
}