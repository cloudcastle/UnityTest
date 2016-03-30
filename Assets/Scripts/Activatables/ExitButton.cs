using UnityEngine;
using UnityEngine.Events;

public class ExitButton : Activatable
{
    public override void Activate(Activator activator) {
        base.Activate(activator);
        GameManager.instance.CompleteLevel();
    }
}