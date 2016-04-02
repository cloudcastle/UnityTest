using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public static class Extensions
{
    public static string Path(this GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }

    public static int ExtMin<T>(this IEnumerable<T> collection, Func<T, int> criteria) {
        if (collection.Count() == 0) {
            return int.MaxValue;
        }
        return collection.Min(criteria);
    }

    public static int ExtMax<T>(this IEnumerable<T> collection, Func<T, int> criteria) {
        if (collection.Count() == 0) {
            return int.MinValue;
        }
        return collection.Max(criteria);
    }

    public static float ExtMin<T>(this IEnumerable<T> collection, Func<T, float> criteria) {
        if (collection.Count() == 0) {
            return float.PositiveInfinity;
        }
        return collection.Min(criteria);
    }

    public static float ExtMax<T>(this IEnumerable<T> collection, Func<T, float> criteria) {
        if (collection.Count() == 0) {
            return float.NegativeInfinity;
        }
        return collection.Max(criteria);
    }

    public static string ExtToString<T>(this IEnumerable<T> collection, string delimiter = ", ") {
        return String.Join(delimiter, collection.Select(obj => obj.ToString()).ToArray());
    }

    public static T CyclicNext<T>(this List<T> list, T obj, int delta = 1) {
        return list[((list.IndexOf(obj) + delta) % list.Count + list.Count) % list.Count];
    }

    public static void ChangeAlpha(this Material material, float alpha) {
        Color c = material.color;
        c.a = alpha;
        material.color = c;
    }

#if UNITY_EDITOR
    public static bool Editor() {
        return Application.isEditor && !EditorApplication.isPlaying;
    }
#else 
    public static bool Editor() {
        return false;
    }
#endif

    public static List<T> GetComponentsInDirectChildren<T>(this Component component) {
        List<T> result = new List<T>();
        foreach (Transform t in component.transform) {
            result = result.Concat(t.GetComponents<T>().ToList()).ToList();
        }
        return result;
    }
}