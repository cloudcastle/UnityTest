using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NodeInstance : Poolable
{
    public Node node = null;

    public BoxCollider bounds;

    public Dictionary<Link, LinkScript> links;

    public int distance;

    bool valid = true;

    public void CreateNode() {
        var nodeObject = GameObject.Instantiate(SampleManager.instance.spaceNode);
        node = nodeObject.GetComponent<Node>();
        node.name = name;
        node.pool = Pool.CreatePool(gameObject);
        node.transform.SetParent(transform.parent);
        transform.SetParent(node.transform);
    }

    public void Bfs(int distance = 0) {
    }

    public void InitLinks() {
        links = new Dictionary<Link, LinkScript>();
        GetComponentsInChildren<LinkScript>().ToList().ForEach(link => {
            links[link.link] = link;
        });
    }

    public void SetLinks() {
        node.links = node.GetComponentsInChildren<Link>().ToList();
    }

    public void Off() {
        valid = false;
        bounds.enabled = false;
    }

    public void On() {
        valid = true;
        bounds.enabled = true;
    }

    public bool IsOn() {
        return valid;
    }

    public void Disconnect() {
        links.Values.ToList().ForEach(link => link.Disconnect());
        
    }
}