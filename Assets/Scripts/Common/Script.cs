using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Script : MonoBehaviour, IPoolable
{
    bool inited = false;
    void Init() {
        if (inited) {
            return;
        }
        inited = true;
        InitInternal();
    }

    public virtual void InitInternal() {
    }

    public virtual void Start() {
        Init();
    }

    public virtual void Taken() {
        Init();
    }

    public virtual void Returned() {
    }
}