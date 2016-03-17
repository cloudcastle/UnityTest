using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public void Awake() {
        instance = this;
    }

    public void CompleteLevel() {
        UI.instance.CompletionScreen();
    }

    public void Replay() {
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void Map() {
        UI.instance.Map();
    }

    void Update() {
        if (Input.GetButtonDown("Replay")) {
            Replay();
        }
    }
}