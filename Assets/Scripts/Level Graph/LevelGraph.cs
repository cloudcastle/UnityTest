using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class LevelGraph : MonoBehaviour
{
    public GameObject nodeSample;
    public GameObject edgeSample;
    public Transform unplacedNodes;
    public Transform nodes;
    public Transform edges;

    [ContextMenu("Update level set")]
    void UpdateLevelSet() {
        if (Extensions.Editor()) {
            var levels = GameManager.game.levels;
            var levelNodes = FindObjectsOfType<LevelNode>().ToList();
            FindObjectsOfType<LevelEdge>().ToList().ForEach(le => DestroyImmediate(le.gameObject));

            levels.ForEach(level => {
                var position = unplacedNodes.position;
                var node = levelNodes.FirstOrDefault(n => n.levelName == level.name);
                if (node != null) {
                    position = node.transform.position;
                DestroyImmediate(node.gameObject);
                }
                var nodeObject = Instantiate(nodeSample) as GameObject;
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
                    Debug.Log("from, to = " + from + to);
                    var edgeObject = Instantiate(edgeSample) as GameObject;
                    var edge = edgeObject.GetComponent<LevelEdge>();
                    edge.from = from;
                    edge.to = to;
                    edge.transform.SetParent(edges);
                });
            });
        }
    }

}