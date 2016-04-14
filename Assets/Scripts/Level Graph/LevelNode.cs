using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LevelNode : MonoBehaviour
{
    public Material baseMaterial;
    public Color baseEmission;
    public Color unlockedEmission;
    public Color completedEmission;
    public float unhoverEmissionMuliplier = 0.7f;
    public float hoverEmissionMuliplier = 2f;

    public TextMesh textMesh;
    public string levelName;
    public bool visible = true;

    new MeshRenderer renderer;

    [NonSerialized] 
    public Level level;

    void OnEnable() {
        this.renderer = GetComponent<MeshRenderer>();
        this.level = GameManager.game.levels.First(l => l.name == levelName);
    }

    void Start() {
        this.level = GameManager.game.levels.First(l => l.name == levelName);
    }

    public bool Hovered() {
        return CameraControl.instance.hovered == this;
    }

    float EmissionMultiplier() {
        return Hovered() ? hoverEmissionMuliplier : unhoverEmissionMuliplier;
    }

    public void SetEmission(Color emission) {
        renderer.material.SetColor("_EmissionColor", emission);
    }

    void Update() {
        if (Extensions.Editor()) {
            gameObject.name = levelName;
            textMesh.text = levelName;
            renderer.enabled = visible;
            UpdateTextMeshSize();
        } else {
            if (level.Completed()) {
                SetEmission(completedEmission * EmissionMultiplier());
            } else if (level.Unlocked()) {
                SetEmission(unlockedEmission * EmissionMultiplier());
            } else {
                SetEmission(baseEmission * EmissionMultiplier());
            }

            UpdateTextMeshSize();
        }
    }

    private void UpdateTextMeshSize() {
        textMesh.fontSize = (int)(1000f / Camera.main.orthographicSize + 1);
        textMesh.transform.localScale /= textMesh.GetComponent<Renderer>().bounds.extents.magnitude * 2;
    }

#if UNITY_EDITOR
    [ContextMenu("Select Children")]
    void UpdateLevelSet() {
        if (Extensions.Editor()) {
            Selection.objects = FindObjectsOfType<LevelEdge>().Where(e => e.from == this).Select(e => e.to.gameObject).ToArray();
        }
    }
#endif
}