using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class ConeConstructor : MonoBehaviour
{
    public string meshName;
    public int axisDivisions = 24;
    public Vector3 size = Vector3.one;

    MeshFilter meshFilter;

    void OnEnable() {
        meshFilter = GetComponent<MeshFilter>();
    }

    Mesh GenerateMesh() {
        var m = new Mesh();
        m.name = name;

        var circleSize = axisDivisions + 1;
        Vector3[] circle = new Vector3[circleSize];

        for (int i = 0; i < axisDivisions; i++) {
            float theta = (360f / axisDivisions * i) * Mathf.Deg2Rad;

            float x = Mathf.Cos(theta) * size.x;
            float z = Mathf.Sin(theta) * size.z;

            circle[i] = new Vector3(x, 0, z);
        }

        circle[circleSize-1] = circle[0];

        var coneCircle = 0 * circleSize;
        var baseCircle = 1 * circleSize;
        Vector3[] v = new Vector3[2 * circleSize + 2];

        for (int i = 0; i < circleSize; i++) {
            v[coneCircle+i] = circle[i];
            v[baseCircle+i] = circle[i];	
        }

        // circle point
        var circlePoint = v.Length - 2;
        v[circlePoint] = Vector3.zero;

        // cone point
        var conePoint = v.Length - 1;
        v[conePoint] = Vector3.up * size.y;

        int[] tris = new int[(axisDivisions * 3) * 2];

        for (int i = 0; i < axisDivisions; i++) {
            // cone sides
            tris[3 * i + 0] = coneCircle + i;
            tris[3 * i + 1] = conePoint;
            tris[3 * i + 2] = coneCircle + i + 1;

            // bottom circle
            tris[3 * axisDivisions + 3 * i + 0] = baseCircle + i + 1;
            tris[3 * axisDivisions + 3 * i + 1] = circlePoint;
            tris[3 * axisDivisions + 3 * i + 2] = baseCircle + i;
        }

        m.vertices = v;
        m.triangles = tris;
        
        Vector2[] uvs = new Vector2[m.vertices.Length];

        int vertexCount = m.vertexCount;

        uvs[vertexCount - 1] = Vector2.zero;
        uvs[vertexCount - 2] = Vector2.zero;

        for (int i = 0; i < m.vertices.Length; i++) {
            uvs[i] = new Vector2(v[i].x, v[i].z);
        }

        m.uv = uvs;

        return m;
    }

    [ContextMenu("Update mesh")]
    void UpdateMesh() {
        if (Extensions.Editor()) {
#if UNITY_EDITOR
            var mesh = GenerateMesh();
            AssetDatabase.CreateAsset(GenerateMesh(), string.Format("Assets/Meshes/Constructed/{0}.asset", name));
            AssetDatabase.SaveAssets();
            meshFilter.mesh = mesh;
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
#endif
        }
    }
}