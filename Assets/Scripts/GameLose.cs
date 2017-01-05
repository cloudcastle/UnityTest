using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLose : MonoBehaviour {
    public static GameLose instance;

    public GameObject lostText;

    public bool gameLost;
    //public float loseTime;
    //public float extraUndoTime = 0.5f;

    public void Lose() {
        if (gameLost) {
            return;
        }
        gameLost = true;
        //loseTime = TimeManager.GameTime;
    }

    public void Awake() {
        instance = this;
    }

    //public bool Undoing() {
    //    if (TimeManager.GameTime < loseTime - extraUndoTime) {
    //        gameLost = false;
    //    }
    //    return gameLost;
    //}

    void Start() {
        //TimeManager.instance.undos.Add(this);
        new BoolTracker(v => gameLost = v, () => gameLost);
    }

    //void Update() {
    //    //lostText.SetActive(false);
    //}
}
