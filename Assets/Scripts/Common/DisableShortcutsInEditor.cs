//using UnityEngine;
//using UnityEditor;

//public class DisableShortcutsInEditor : MonoBehaviour
//{
//    static void blocked(string item)
//    {
//        Debug.Log(item);
//        if (!Application.isPlaying)
//        {
//            EditorApplication.ExecuteMenuItem(item);
//        }
//    }

//    [MenuItem("Disabled in Play Mode/Scene %1")]
//    static void WindowScene()
//    {
//        blocked("Window/Scene");
//    }

//    [MenuItem("Disabled in Play Mode/Save Scene %s")]
//    static void SaveScene()
//    {
//        blocked("File/Save Scene");
//    }

//    [MenuItem("Disabled in Play Mode/Select All %a")]
//    static void SelectAll()
//    {
//        blocked("Edit/Select All");
//    }

//    [MenuItem("Disabled in Play Mode/Duplicate %d")]
//    static void Duplicate()
//    {
//        blocked("Edit/Duplicate");
//    }

//    void OnGuI()
//    {
//        Event.current.Use();
//    }
//}