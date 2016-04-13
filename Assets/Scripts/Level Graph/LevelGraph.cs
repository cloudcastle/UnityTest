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
            var levelEdges = FindObjectsOfType<LevelEdge>().ToList();
            levels.ForEach(level => {
                if (levelNodes.Any(n => n.levelName == level.name)) {
                    return;
                }
                var nodeObject = Instantiate(nodeSample) as GameObject;
                var node = nodeObject.GetComponent<LevelNode>();
                node.levelName = level.name;
                levelNodes.Add(node);
                node.transform.position = unplacedNodes.position;
                node.transform.SetParent(nodes);
            });
            levels.ForEach(level => {
                var to = levelNodes.Find(n => n.levelName == level.name);
                level.dependencies.ForEach(dependency => {
                    var from = levelNodes.Find(n => n.levelName == dependency.name);
                    if (levelEdges.Any(e => e.from == from && e.to == to)) {
                        return;
                    }
                    var edgeObject = Instantiate(edgeSample) as GameObject;
                    var edge = edgeObject.GetComponent<LevelEdge>();
                    edge.from = from;
                    edge.to = to;
                    levelEdges.Add(edge);
                    edge.transform.SetParent(edges);
                });
            });
            levelEdges.ForEach(e => {
                if (!levels.Find(l => l.name == e.to.levelName).dependencies.Any(d => d.name == e.from.levelName)) {
                    Destroy(e.gameObject);
                }
            });
        }
    }

}