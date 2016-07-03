using UnityEngine;
using System;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class Pictured : MonoBehaviour
{
    public Texture picture;

    MeshRenderer meshRenderer;

    Item item;

    void OnEnable() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Awake() {
        this.item = GetComponent<Item>();
    }

    public void Update() {
        if (!enabled) {
            return;
        }
        if (Extensions.Editor()) {
            if (meshRenderer.sharedMaterials.Length >= 2) {
                var materials = meshRenderer.sharedMaterials;
                var pictureMaterial = materials[1];
                var texture = pictureMaterial.GetTexture("_MainTex");
                Debug.LogFormat("texture is {0} (eq = {1})", texture, texture == this.picture);
                if (texture != picture) {
                    var newPictureMaterial = new Material(pictureMaterial);
                    newPictureMaterial.SetTexture("_MainTex", picture);
                    materials[1] = newPictureMaterial;
                    meshRenderer.materials = materials;
                    Debug.LogFormat("Pictured texture updated");
                }
            }
        } 
    }

    void FixedUpdate() {
        AdjustRotation();
    }

    private void AdjustRotation() {
        if (item == null) {
            return;
        }
        if (item.IsPicked) {
            transform.localRotation = Quaternion.identity;
            return;
        }
        if (Player.instance.current.activator.current == item) {
            var finalRotation = Quaternion.LookRotation(Player.instance.current.activator.transform.position - transform.position);
            finalRotation *= Quaternion.Euler(Vector3.down * 90);
            transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, Time.fixedDeltaTime * 5);
        }
    }
}
