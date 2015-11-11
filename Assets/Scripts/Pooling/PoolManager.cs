using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    void Awake() {
        instance = this;
    }
}