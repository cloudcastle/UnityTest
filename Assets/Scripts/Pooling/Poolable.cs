using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Poolable : MonoBehaviour
{
    public Pool pool;

    void ReturnToPool() {
        pool.Return(gameObject);
    }
}