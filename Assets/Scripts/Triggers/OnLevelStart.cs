using UnityEngine;
using System.Collections;

public class OnLevelStart : AbstractTrigger
{
    bool firstUpdate = true;

    void Update() {
        if (!firstUpdate) {
            return;
        }
        firstUpdate = false;
        effect.Run();
    }
}