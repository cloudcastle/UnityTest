using UnityEngine;
using System.Collections;

public class OnLevelStart : AbstractTrigger
{
    public bool dropUndo = true;

    void Start() {
        effect.Run().Then(() => Undo.instance.DropUndoData());
    }
}