using UnityEngine;
using System.Collections;
using RSG;

public class PickEffect : Effect
{
    public Item target;
    public bool animate = true;

    public override IPromise Run() {
        Debug.Log("Pick effect run");
        return Player.current.inventory.Pick(target, animate: animate);
    }
}