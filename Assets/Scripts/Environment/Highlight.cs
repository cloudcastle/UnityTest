﻿using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Highlight : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Color underSightEmission = new Color(0.4f, 0.4f, 0.4f);
    Color baseEmission = Color.black;

    public bool on;

    protected virtual void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Switch(bool on) {
        this.on = on;
        meshRenderer.material.SetColor("_EmissionColor", on ? underSightEmission : baseEmission);
    }
}