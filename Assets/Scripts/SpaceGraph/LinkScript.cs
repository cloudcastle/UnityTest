using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LinkScript : MonoBehaviour
{
    public Link link;

    public Node node {
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
}