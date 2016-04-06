﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TimeManager))]
public class CursorLock : MonoBehaviour
{
    bool lockCursor {
        get {
            return !TimeManager.Paused;
        }
    }

    void AssureCursorState(CursorLockMode lockMode, bool visibility) {
        if (Cursor.lockState != lockMode) {
            Cursor.lockState = lockMode;
        }
        if (Cursor.visible != visibility) {
            Cursor.visible = visibility;
        }
    }

    void Update() {
        if (lockCursor) {
            AssureCursorState(CursorLockMode.Locked, false);
        } else {
            AssureCursorState(CursorLockMode.None, true);
        }
    }
}