using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Poolable : MonoBehaviour
{
    public bool appeared = false;

    public Pool pool;

    public void ReturnToPool() {
        pool.Return(this);
    }
}