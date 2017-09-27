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
    public Color inverseDependencyBaseEmission;
    public Color inverseDependencyUnlockedEmission;
    public Color inverseDependencyCompletedEmission;
    public Color lockedBySelected;
    public float unhoverEmissionMuliplier = 0.7f;
    public float hoverEmissionMuliplier = 2f;

    public TextMesh textMesh;
    public SpriteRenderer ball;
    public SpriteRenderer light;
    public GameObject highLight;

    public string levelName;
    public bool visible = true;

    public bool autoText = true;
    public bool autoScaleText = true;

    public Vector3 basePosition;
    public Vector3 baseScale;

    MeshRenderer textRenderer;

    bool inited = false;

    [NonSerialized] 
    public Level level;

    void OnEnable() {
        this.textRenderer = textMesh.GetComponent<MeshRenderer>();
#if UNITY_EDITOR
        Selection.selectionChanged += OnSelectionChanged;
#endif
    }

#if UNITY_EDITOR
    void OnDisable() {
        Selection.selectionChanged -= OnSelectionChanged;
    }
#endif

    public void Start() {
        this.level = GameManager.game.levels.First(l => l.name == levelName);
        if (!inited) {
            inited = true;
            basePosition = transform.position * 2f;
            baseScale = transform.localScale;
        }
        Update();
    }

    public bool Hovered() {
        return CameraControl.instance.hovered == this;
    }

    float EmissionMultiplier() {
        return Hovered() ? hoverEmissionMuliplier : unhoverEmissionMuliplier;
    }

    public void SetEmission(Color emission) {
//        if (Extensions.Editor()) {
//            var tempMaterial = new Material(renderer.sharedMaterial);
//            tempMaterial.SetColor("_EmissionColor", emission);
//            //renderer.sharedMaterial = tempMaterial;
//        } else {
//            //renderer.material.SetColor("_EmissionColor", emission);
//        }
    }

#if UNITY_EDITOR
    void OnSelectionChanged() {
        this.level = GameManager.game.levels.FirstOrDefault(l => l.name == levelName);
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

    public bool Unlocked() {
        return level.Unlocked() || Cheats.on;
    }

    public void SetVisible(bool visible) {
        ball.enabled = textRenderer.enabled = light.enabled = visible;
        highLight.SetActive(visible && IsHovered2());
    }

    public bool IsHovered() {
        return CameraControl.instance.hovered == this;
    }

    public bool IsHovered2() {
        if (IsHovered()) {
            return true;
        }
        if (CameraControl.instance.hovered == null) {
            return false;
        }
        return CameraControl.instance.hovered.level.dependencies.Contains(level) || level.dependencies.Contains(CameraControl.instance.hovered.level);
    }

    public bool IsVisible() {
        return visible && Unlocked();
    }

    void Update() {
        if (Extensions.Editor()) {
            this.level = GameManager.game.levels.FirstOrDefault(l => l.name == levelName);
            gameObject.name = levelName;
            if (autoText) {
                textMesh.text = levelName;
            }
            UpdateTextMeshSize();
        } else {
            SetEmission(Emission());
            SetVisible(IsVisible());
            UpdateTextMeshSize();

            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 center = CameraControl.instance.hovered == null ? new Vector3(float.PositiveInfinity, float.PositiveInfinity, 0) : CameraControl.instance.hovered.basePosition;
            float distance = Vector3.Distance(center, basePosition);
            float scale = 1;
            Vector3 pos = basePosition;
            if (CameraControl.instance.hovered) {
                if (CameraControl.instance.hovered == this) {
                    scale = 3;
                    pos = mouse.Change(z: pos.z);
                }           
                if (CameraControl.instance.hovered.level.dependencies.Contains(level) || level.dependencies.Contains(CameraControl.instance.hovered.level)) {
                    scale = 1.7f;
                    pos += (CameraControl.instance.hovered.transform.position - basePosition) * 0.1f;
                }
            }

            transform.position = Vector3.Lerp(pos, transform.position, Mathf.Pow(0.5f, Time.deltaTime*30));
            transform.localScale = Vector3.Lerp(Vector3.one * scale, transform.localScale, Mathf.Pow(0.5f, Time.deltaTime*30));
        }
    }

    private void UpdateTextMeshSize() {
        textMesh.transform.localScale = Vector3.one * 0.00356f;
        if (!autoScaleText) {
            return;
        }
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

    [ContextMenu("Hide all levels but one")]
    void HideAll() {
        FindObjectsOfType<LevelNode>().ForEach(node => {
            if (node.level.dependencies.Count > 0) {
                node.visible = false;
            }
        });
    }

    [ContextMenu("Show all levels unlocked by visible")]
    void ShowUnlocked() {
        List<LevelNode> nextTier = FindObjectsOfType<LevelNode>().Where(node => !FindObjectsOfType<LevelNode>().Any(node2 => !node2.visible && node.level.dependencies.Contains(node2.level))).ToList();
        nextTier.ForEach(node => node.visible = true);
    }
#endif
}