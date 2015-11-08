using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public static GameObject instance;

    void Awake()
    {
        instance = gameObject;
    }
}