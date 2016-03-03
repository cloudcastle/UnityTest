using UnityEngine;
using System.Collections;

public class Activator : MonoBehaviour
{
    void Update()
    {
        if (PauseManager.paused)
        {
            return;
        }
        if (Input.GetButtonDown("Activate")) {
            if (Eye.instance.underSight != null) {
                var activatable = Eye.instance.underSight.GetComponent<Activatable>();
                if (activatable != null) {
                    activatable.Activate();
                }
            }
        }
    }
}