using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public class ReplaceToPrefabs : MonoBehaviour
{
    public Jumper jumperPrefab;

    [ContextMenu("Replace to prefabs")]
    public void Replace() {
//        var jumper = PrefabUtility.InstantiatePrefab(jumperPrefab) as Jumper;
//        jumper.transform.position = new Vector3(-14, 0.1f, 22);
//        //Instantiate(jumperPrefab, new Vector3(-14, 0.1f, 22), Quaternion.identity);
//        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        FindObjectsOfType<Jumper>().ForEach(j => {
            if (PrefabUtility.GetPrefabParent(j) == null) {
                Debug.LogFormat("Replacing jumper at {0} to prefab", j.transform.position);
                var jumper = PrefabUtility.InstantiatePrefab(jumperPrefab) as Jumper;
                jumper.transform.SetParent(j.transform.parent);
                jumper.transform.position = j.transform.position;
                jumper.transform.rotation = j.transform.rotation;
                DestroyImmediate(j);
            }
        });
        Debug.LogFormat("Done");
    }
}

#endif

