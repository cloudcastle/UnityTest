using UnityEngine;
using System.Collections;
using System;
using RSG;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class Disappear : Effect
{
    public override IPromise Run()
    {
        if (!Ready()) {
            return Promise.Resolved();
        }
        gameObject.SetActive(false);
        return Promise.Resolved();
    }

    bool Ready() {
        return gameObject.activeSelf;
    }

    public override ActivatableStatus Status() {
        return Ready() ? ActivatableStatus.Activatable : ActivatableStatus.Activated;
    }
}