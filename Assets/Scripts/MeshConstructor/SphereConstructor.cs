using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class SphereConstructor : MonoBehaviour
{
    public string meshName;
    public int latitudeSpans = 12;
    public int longitudeSpans = 24;
    public int halfLatitudePictureSpans = 2;
    public int halfLongitudePictureSpans = 2;
    public Vector3 size = Vector3.one/2;

    int latitudes;
    int longitudes;
    int pictureLatitudeSpans;
    int pictureLongitudeSpans;
    int pictureLatitudes;
    int pictureLongitudes;


    bool[,] pictureSpans;

    MeshVertex[,] baseMatrix;
    MeshVertex[,] pictureMatrix;
    
    MeshFilter meshFilter;

    void OnEnable() {
        meshFilter = GetComponent<MeshFilter>();
    }

    Vector3 getSpherePoint(int latitudeIndex, int longitudeIndex) {
        float phi = 2 * Mathf.PI * longitudeIndex / longitudeSpans;
        float omega = -Mathf.PI / 2 + Mathf.PI * latitudeIndex / latitudeSpans;

        float horizontal = Mathf.Cos(omega);
        float vertical = Mathf.Sin(omega);

        float x = horizontal * Mathf.Cos(phi) * size.x;
        float y = vertical * size.y;
        float z = horizontal * Mathf.Sin(phi) * size.z;

        return new Vector3(x, y, z);
    }

    bool isPictureSpan(int latitudeSpanIndex, int longitudeSpanIndex) {
        return pictureSpans[latitudeSpanIndex, longitudeSpanIndex];
    }

    void AddSpan(MeshConstructor mc, MeshVertex[,] mx, int latitudeSpanIndex, int longitudeSpanIndex, int submesh = 0) {                
        MeshVertex topLeft = mx[latitudeSpanIndex + 1, longitudeSpanIndex];
        MeshVertex topRight = mx[latitudeSpanIndex + 1, longitudeSpanIndex + 1];
        MeshVertex bottomLeft = mx[latitudeSpanIndex, longitudeSpanIndex];
        MeshVertex bottomRight = mx[latitudeSpanIndex, longitudeSpanIndex + 1];
        mc.AddTriangle(topLeft, topRight, bottomLeft, submesh);
        mc.AddTriangle(bottomRight, bottomLeft, topRight, submesh);
    }

    Mesh GenerateMesh() {
        var mc = new MeshConstructor();
        mc.mesh.name = name;
        mc.SetSubmeshCount(2);

        latitudes = latitudeSpans + 1;
        longitudes = longitudeSpans + 1;
        pictureLatitudeSpans = 2 * halfLatitudePictureSpans;
        pictureLongitudeSpans = 2 * halfLongitudePictureSpans;
        pictureLatitudes = pictureLatitudeSpans + 1;
        pictureLongitudes = pictureLongitudeSpans + 1;

        pictureSpans = new bool[latitudeSpans, longitudeSpans];
        baseMatrix = new MeshVertex[latitudes, longitudes];
        pictureMatrix = new MeshVertex[latitudes, longitudes];

        for (int latitudePictureSpanIndex = 0; latitudePictureSpanIndex < pictureLatitudeSpans; latitudePictureSpanIndex++) {
            for (int longitudePictureSpanIndex = 0; longitudePictureSpanIndex < pictureLongitudeSpans; longitudePictureSpanIndex++) {
                int latitudeSpanIndex = latitudes/2-halfLatitudePictureSpans+latitudePictureSpanIndex;
                int longitudeSpanIndex = Extensions.Modulo(-halfLongitudePictureSpans+longitudePictureSpanIndex, longitudeSpans);
                pictureSpans[latitudeSpanIndex, longitudeSpanIndex] = true;
            }
        }

        for (int latitudeIndex = 0; latitudeIndex <= latitudeSpans; latitudeIndex++) {
            for (int longitudeIndex = 0; longitudeIndex <= longitudeSpans; longitudeIndex++) {
                baseMatrix[latitudeIndex, longitudeIndex] = new MeshVertex(
                    position: getSpherePoint(latitudeIndex, longitudeIndex),
                    uv: new Vector2(1f * longitudeIndex / longitudeSpans, 1f * latitudeIndex / latitudeSpans),
                    normal: getSpherePoint(latitudeIndex, longitudeIndex).normalized
                );
            }
        }

        for (int pictureLatitudeIndex = 0; pictureLatitudeIndex <= 2 * halfLatitudePictureSpans; pictureLatitudeIndex++) {
            for (int pictureLongitudeIndex = 0; pictureLongitudeIndex <= 2 * halfLongitudePictureSpans; pictureLongitudeIndex++) {
                int latitudeIndex = latitudes/2 -halfLatitudePictureSpans + pictureLatitudeIndex;
                int longitudeIndex = Extensions.Modulo(-halfLongitudePictureSpans + pictureLongitudeIndex, longitudeSpans);

                //Debug.LogFormat("Adding picture vertex {0}, {1}", latitudeIndex, longitudeIndex);
                pictureMatrix[latitudeIndex, longitudeIndex] = new MeshVertex(
                    position: getSpherePoint(latitudeIndex, longitudeIndex),
                    uv: new Vector2(1f * pictureLongitudeIndex / pictureLongitudeSpans, 1f * pictureLatitudeIndex / pictureLatitudeSpans),
                    normal: getSpherePoint(latitudeIndex, longitudeIndex).normalized
                );

                if (longitudeIndex == 0) {
                    longitudeIndex = longitudes - 1;
                    //Debug.LogFormat("Adding picture vertex {0}, {1}", latitudeIndex, longitudeIndex);
                    pictureMatrix[latitudeIndex, longitudeIndex] = new MeshVertex(
                        position: getSpherePoint(latitudeIndex, longitudeIndex),
                        uv: new Vector2(1f * pictureLongitudeIndex / pictureLongitudeSpans, 1f * pictureLatitudeIndex / pictureLatitudeSpans),
                        normal: getSpherePoint(latitudeIndex, longitudeIndex).normalized
                    );
                }
            }
        }

        for (int latitudeSpanIndex = 0; latitudeSpanIndex < latitudeSpans; latitudeSpanIndex++) {
            for (int longitudeSpanIndex = 0; longitudeSpanIndex < longitudeSpans; longitudeSpanIndex++) {
                if (isPictureSpan(latitudeSpanIndex, longitudeSpanIndex)) {
                    AddSpan(mc, pictureMatrix, latitudeSpanIndex, longitudeSpanIndex, 1);
                }
                AddSpan(mc, baseMatrix, latitudeSpanIndex, longitudeSpanIndex);
            }
        }

        return mc.Done();
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