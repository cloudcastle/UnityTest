using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public class RebuildLevelGraph : MonoBehaviour
{
    [ContextMenu("RebuildLevelGraph")]
    public void Replace() {
        FindObjectOfType<LevelGraph>().UpdateLevelSet();
    }
}

#endif

