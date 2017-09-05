using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameTimeClock : MonoBehaviour {
    public Text text;

    void Update() {
        text.color = TimeManager.instance.Undoing() ? Color.black : Color.white;
    }
}
