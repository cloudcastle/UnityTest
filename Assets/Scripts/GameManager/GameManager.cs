using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public void Awake() {
        instance = this;
    }

    public void CompleteLevel() {
        UI.instance.completionScreen.Show();
    }

    public void Replay() {
        Application.LoadLevel(Application.loadedLevelName);
    }

    void Update() {
        if (Input.GetButtonDown("Replay")) {
            Replay();
        }
    }
}