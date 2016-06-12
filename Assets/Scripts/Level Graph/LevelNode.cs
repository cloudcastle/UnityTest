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
    public Color dependencyBaseEmission;
    public Color dependencyUnlockedEmission;
    public Color dependencyCompletedEmission;
    public Color lockedBySelected;
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
#if UNITY_EDITOR
        Selection.selectionChanged += OnSelectionChanged;
#endif
    }

#if UNITY_EDITOR
    void OnDisable() {
        Selection.selectionChanged -= OnSelectionChanged;
    }
#endif

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
        if (Extensions.Editor()) {
            var tempMaterial = new Material(renderer.sharedMaterial);
            tempMaterial.SetColor("_EmissionColor", emission);
            renderer.sharedMaterial = tempMaterial;
        } else {
            renderer.material.SetColor("_EmissionColor", emission);
        }
    }

#if UNITY_EDITOR
    void OnSelectionChanged() {
        this.level = GameManager.game.levels.First(l => l.name == levelName);
        GameManager.game.levels.ForEach(l => l.transitiveDependencies = null);
        Level selectedLevel = null;
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<LevelNode>() != null) {
            selectedLevel = Selection.activeGameObject.GetComponent<LevelNode>().level;
        }
        if (selectedLevel != null && selectedLevel.Depends(level)) {
            SetEmission(completedEmission);
        } else if (selectedLevel == level) {
            SetEmission(unlockedEmission);
        } else if (selectedLevel != null && level.Depends(selectedLevel)) {
            SetEmission(lockedBySelected);
        } else {
            SetEmission(baseEmission);
        }
    }
#endif

    public Color Emission() {
        if (CameraControl.instance.hovered != null && CameraControl.instance.hovered.level.dependencies.Contains(level)) {
            if (level.Completed()) {
                return dependencyCompletedEmission * EmissionMultiplier();
            } else if (level.Unlocked()) {
                return dependencyUnlockedEmission * EmissionMultiplier();
            } else {
                return dependencyBaseEmission * EmissionMultiplier();
            }
        } else {
            if (level.Completed()) {
                return completedEmission * EmissionMultiplier();
            } else if (level.Unlocked()) {
                return unlockedEmission * EmissionMultiplier();
            } else {
                return baseEmission * EmissionMultiplier();
            }
        }
    }

    void Update() {
        if (Extensions.Editor()) {
            this.level = GameManager.game.levels.First(l => l.name == levelName);
            gameObject.name = levelName;
            textMesh.text = levelName;
            renderer.enabled = visible;
            UpdateTextMeshSize();
        } else {
            SetEmission(Emission());

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