using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class UndoImageEffect : MonoBehaviour
{
    public PostEffectsBase effect;

    void Update() {
        effect.enabled = TimeManager.instance.Undoing() && false;
    }
}