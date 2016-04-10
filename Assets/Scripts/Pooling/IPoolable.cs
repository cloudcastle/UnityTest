using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IPoolable
{
    void Taken();
    void Returned();
}