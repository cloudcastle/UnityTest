using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class PortalBox : MonoBehaviour
{
    public Portal portal;

    void FixedUpdate() {
        PortalBall ball = null;
        Collider[] inside = Physics.OverlapSphere(transform.position, 0.05f, LayerMask.GetMask("Item"));
        //Debug.LogFormat("inside = {0}", inside.ExtToString());
        ball = inside.Select(c => c.GetComponent<PortalBall>()).FirstOrDefault(pb => pb != null);
        //Debug.LogFormat("ball = {0}", ball);
        if (ball != null) {
            //Debug.LogFormat("Connect {0} to {1}", portal, ball.portal);
            portal.Connect(ball.portal);
        } else {
            portal.Disconnect();
        }
    }
}
