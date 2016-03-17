using UnityEngine;
using System.Collections;

public class Picker : MonoBehaviour
{
    public Transform worldCamera;
    public Transform inventoryCamera;

    void Update()
    {
        if (TimeManager.paused)
        {
            return;
        }
        if (Input.GetButton("Pick")) {
            if (Eye.instance.underSight != null) {
                var item = Eye.instance.underSight.GetComponent<Item>();
                if (item != null) {
                    Pick(item);
                }
            }
        }
    }

    void Pick(Item item) {
        item.transform.SetParent(worldCamera, worldPositionStays: true);
        item.transform.SetParent(inventoryCamera, worldPositionStays: false);
    }
}