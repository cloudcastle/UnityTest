using UnityEngine;
using System.Collections;

public class PickEffect : Effect
{
    public Item target;

    public override bool Run() {
        Player.current.inventory.Pick(target);
        return true;
    }
}