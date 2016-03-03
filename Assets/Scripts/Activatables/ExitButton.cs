using UnityEngine;
using UnityEngine.Events;

public class ExitButton : Activatable
{
    public override void Activate() {
        base.Activate();
        GameManager.instance.CompleteLevel();
    }
}