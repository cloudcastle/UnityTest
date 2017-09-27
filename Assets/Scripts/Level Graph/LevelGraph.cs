using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using System.Linq;

[ExecuteInEditMode]
public class LevelGraph : MonoBehaviour
{
    public GameObject nodeSample;
    public GameObject edgeSample;
    public Transform unplacedNodes;
    public Transform nodes;
    public Transform edges;

    public float zoom = 1;

    [ContextMenu("Zoom")]
    void Zoom() {
        if (Extensions.Editor()) {
            #if UNITY_EDITOR
            var levelNodes = FindObjectsOfType<LevelNode>().ToList();
            levelNodes.ForEach(n => {
                n.transform.position *= zoom;
            });
            zoom = 1;
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            #endif
        }
    }

    [ContextMenu("Update level set")]
    public void UpdateLevelSet() {
        if (Extensions.Editor()) {
#if UNITY_EDITOR
            GameManager.game = new Game();
            var levels = GameManager.game.levels;
            var levelNodes = FindObjectsOfType<LevelNode>().ToList();
            var visibleNodes = levelNodes.Where(n => n.visible).Select(n => n.level.name).ToList();
            FindObjectsOfType<LevelEdge>().ToList().ForEach(le => DestroyImmediate(le.gameObject));

            levels.ForEach(level => {
                var position = unplacedNodes.position;
                var node = levelNodes.FirstOrDefault(n => n.levelName == level.name);
                if (node != null) {
                    position = node.transform.position;
                    DestroyImmediate(node.gameObject);
                }
                var nodeObject = PrefabUtility.InstantiatePrefab(nodeSample) as GameObject;
                node = nodeObject.GetComponent<LevelNode>();
                node.levelName = level.name;
                node.transform.position = position;
                node.transform.SetParent(nodes);
            });
            levelNodes = FindObjectsOfType<LevelNode>().ToList();
            levels.ForEach(level => {
                var to = levelNodes.Find(n => n.levelName == level.name);
                level.dependencies.ForEach(dependency => {
                    var from = levelNodes.Find(n => n.levelName == dependency.name);
                    var edgeObject = PrefabUtility.InstantiatePrefab(edgeSample) as GameObject;
                    var edge = edgeObject.GetComponent<LevelEdge>();
                    edge.from = from;
                    edge.to = to;
                    edge.transform.SetParent(edges);
                });
            });
            levelNodes.Where(n => !visibleNodes.Contains(n.levelName)).ForEach(n => n.visible = false);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
#endif
        }
    }

}