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

public class AppearOrDisappear : Effect
{
    bool ready = true;

    public override IPromise Run()
    {
        if (!Ready()) {
            return Promise.Resolved();
        }
        gameObject.SetActive(!gameObject.activeSelf);
        ready = false;
        TimeManager.WaitFor(0.5f).Then(() => ready = true).Done();
        return Promise.Resolved();
    }

    bool Ready() {
        return ready;
    }

    public override ActivatableStatus Status() {
        return Ready() ? ActivatableStatus.Activatable : ActivatableStatus.Activated;
    }
}