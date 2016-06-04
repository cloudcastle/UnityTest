using UnityEngine;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class MeshConstructor
{
    public Mesh mesh;

    public List<int>[] triangles = new List<int>[10];

    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector2> uv = new List<Vector2>();
    public List<Vector3> normals = new List<Vector3>();

    public void SetSubmeshCount(int cnt) {
        mesh.subMeshCount = cnt;
        for (int i = 0; i < cnt; i++) {
            triangles[i] = new List<int>();
        }
    }

    public MeshConstructor() {
        mesh = new Mesh();
    }

    public void AddVertex(MeshVertex v) {
        if (v.index != -1) {
            return;
        }
        vertices.Add(v.position);
        uv.Add(v.uv);
        v.index = vertices.Count - 1;
        normals.Add(v.normal);
    }

    public void AddTriangle(MeshVertex a, MeshVertex b, MeshVertex c, int submeshIndex = 0) {
        AddVertex(a);
        AddVertex(b);
        AddVertex(c);
        triangles[submeshIndex].Add(a.index);
        triangles[submeshIndex].Add(b.index);
        triangles[submeshIndex].Add(c.index);
    }

    public Mesh Done() {
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.normals = normals.ToArray();
        for (int i = 0; i < mesh.subMeshCount; i++) {
            mesh.SetTriangles(triangles[i], i);
        }
        return mesh;
    }
}