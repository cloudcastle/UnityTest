using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Poolable : MonoBehaviour
{
    public bool exists = false;

    public Pool pool;

    public void ReturnToPool() {
        pool.Return(this);
    }

    public void ReturnToPoolLight() {
        pool.ReturnLight(this);
    }

    public virtual void Instantiated() {
    }
}