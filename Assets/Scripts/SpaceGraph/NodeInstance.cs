using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NodeInstance : Poolable
{
    public Node node = null;

    public BoxCollider bounds;

    public Dictionary<Link, LinkScript> links;
    public List<LinkScript> linksList;

    bool on = true;

    public int distance;

    public void CreateNode() {
        var nodeObject = GameObject.Instantiate(SampleManager.instance.spaceNode);
        node = nodeObject.GetComponent<Node>();
        node.name = name;
        node.pool = Pool.CreatePool(gameObject);
        node.transform.SetParent(transform);
        node.transform.Reset();
        node.transform.SetParent(transform.parent, worldPositionStays: true);
        transform.SetParent(node.transform);
    }

    public override void Instantiated() {
        base.Instantiated();
        InitLinks();
    }

    void InitLinks() {
        links = new Dictionary<Link, LinkScript>();
        linksList = GetComponentsInChildren<LinkScript>().ToList();
        linksList.ForEach(link => {
            links[link.link] = link;
        });
    }

    public void SetLinks() {
        node.links = node.GetComponentsInChildren<Link>().ToList();
    }

    public void Off() {
        on = false;
    }

    public void On() {
        on = true;
    }

    public bool IsOn() {
        return on;
    }

    public void Disconnect() {
        linksList.ForEach(link => link.Disconnect());
    }
}