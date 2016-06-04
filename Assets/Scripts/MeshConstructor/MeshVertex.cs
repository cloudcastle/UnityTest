using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
#endif

public class MeshVertex
{
    public Vector3 position;
    public Vector2 uv;
    public Vector3 normal;
    public int index = -1;

    public MeshVertex(Vector3 position, Vector2 uv, Vector3 normal) {
        this.position = position;
        this.uv = uv;
        this.normal = normal;
    }

    public int GetIndex(MeshConstructor mc) {
        return index;
    }
}