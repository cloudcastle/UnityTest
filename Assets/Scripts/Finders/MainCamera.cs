using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{
    public static GameObject instance;

    void Awake()
    {
        instance = gameObject;
    }
}