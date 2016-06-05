using UnityEngine;
using System.Collections;
using RSG;

public class SetTimeEffect : Effect
{
    public float gameTime;

    public override IPromise Run() {
        Debug.Log("Create past effect run");
        TimeManager.instance.gameTime = gameTime;
        TimeManager.instance.stoppableGameTime = gameTime;
        return Promise.Resolved();
    }
}