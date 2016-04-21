using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class SampleManager : MonoBehaviour
{
    public static SampleManager instance;

    public GameObject spaceNode;
    public GameObject spaceLink;

    public void Awake() {
        instance = this;
    }
}