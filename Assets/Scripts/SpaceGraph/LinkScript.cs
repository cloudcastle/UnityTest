using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LinkScript : MonoBehaviour
{
    public Link link;

    public Node Node {
        get {
            return link.to;
        }
    }

    public NodeInstance to;
    public LinkScript backLink;

    public void CreateLink() {
        var linkObject = GameObject.Instantiate(SampleManager.instance.spaceLink);
        linkObject.transform.SetParent(GetComponentInParent<Node>().linkParent);
        link = linkObject.GetComponent<Link>();
        link.name = name;
        link.to = to.node;
        to = null;
    }

    public void SetBackLink() {
        link.backLink = backLink.link;
        backLink = null;
    }

    public void Disconnect() {
        if (to != null) {
            to.links[link.backLink].to = null;
            to = null;
        }
    }

    public bool AssertAcceptable() {
        var acceptable = SpaceGraph.Acceptable(to, this);
        if (!acceptable) {
            Debug.LogError(string.Format("link.to is not acceptable: {0} for {1}", to, transform.Path()));
            UnityEditor.EditorApplication.isPaused = true;
        }
        return acceptable;
    }
}