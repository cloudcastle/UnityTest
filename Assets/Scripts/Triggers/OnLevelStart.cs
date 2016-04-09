using UnityEngine;
using System.Collections;

public class OnLevelStart : AbstractTrigger
{
    public bool dropUndo = true;

    bool firstUpdate = true;

    void Update() {
        if (!firstUpdate) {
            return;
        }
        firstUpdate = false;
        effect.Run();
        if (dropUndo) {
            FindObjectOfType<Undo>().DropUndoData();
        }
    }
}