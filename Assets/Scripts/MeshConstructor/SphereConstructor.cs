using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class SphereConstructor : MonoBehaviour
{
    public string meshName;
    public int axisDivisions = 24;
    public Vector3 size = Vector3.one;

    MeshFilter meshFilter;

    void OnEnable() {
        meshFilter = GetComponent<MeshFilter>();
    }

    Mesh ChangeMesh() {
        var m = meshFilter.sharedMesh;

        //Debug.LogFormat("m.uv = {0}", m.uv.ToList().ExtToString(elementToString: x => x.ExtToString()));
        m.uv = m.uv.Select(p => {
            Vector2 scaled = (((p - Vector2.one / 2) * 4) + Vector2.one / 2);
            //return new Vector2(Mathf.Clamp(scaled.x, 0, 1), Mathf.Clamp(scaled.y, 0, 1));
            return scaled;
        }).ToArray();
        return m;
    }

#if UNITY_EDITOR
    [ContextMenu("Change texture coordinates")]
    void UpdateMesh() {
        if (Extensions.Editor()) {
            var mesh = ChangeMesh();
            //AssetDatabase.CreateAsset(GenerateMesh(), string.Format("Assets/Meshes/Constructed/{0}.asset", name));
            //AssetDatabase.SaveAssets();
            //meshFilter.mesh = mesh;
            //EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
#endif
}