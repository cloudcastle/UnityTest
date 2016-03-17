using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static Game game = new Game();

    public static GameManager instance;

    public void Awake() {
        instance = this;
    }

    public Level CurrentLevel() {
        return game.levels.FirstOrDefault(level => level.name == Application.loadedLevelName);
    }

    public void CompleteLevel() {
        var level = CurrentLevel();
        level.completed = true;
        UI.instance.CompletionScreen();
    }

    public void Replay() {
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void Play(Level level) {
        Application.LoadLevel(level.name);
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