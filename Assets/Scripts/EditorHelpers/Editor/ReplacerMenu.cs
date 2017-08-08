using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor;

public class ReplacerMenu : Editor
{
    [MenuItem("Custom/Replace To Prefabs/Jumpers")]
    private static void ReplaceJumpersToPrefabs() {
        FindObjectOfType<ReplaceToPrefabs>().Replace();
    }
}


