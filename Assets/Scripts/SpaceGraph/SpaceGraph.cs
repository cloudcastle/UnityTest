using UnityEngine;
using System.Collections;
using System.Linq;

public class SpaceGraph : MonoBehaviour
{
    const int distanceLimit = 10;

    public static SpaceGraph instance;

    public NodeInstance current;

    void Awake() {
        instance = this;
        current.PreprocessConnectivityComponent();
    }

    void Start() {
        current = current.node.Instantiate(transform);
        current.Dfs(distanceLimit);
    }
}