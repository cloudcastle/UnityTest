using UnityEngine;
using System.Collections;

public class TransformState
{
    public Vector3 position;
    public Vector3 scale;

    public TransformState(Vector3 position, Vector3 scale) {
        this.position = position;
        this.scale = scale;
    }
}