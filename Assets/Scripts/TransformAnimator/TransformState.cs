using UnityEngine;
using System.Collections;

public class TransformState
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformState(Vector3 position, Quaternion? rotation = null, Vector3? scale= null) {
        this.position = position;
        this.rotation = rotation ?? Quaternion.identity;
        this.scale = scale ?? Vector3.one;
    }
}