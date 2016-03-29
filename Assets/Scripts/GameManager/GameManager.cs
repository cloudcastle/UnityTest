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
        game.completedLevels.Add(level);
        UI.instance.CompletionScreen();
    }

    public void Replay() {
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void PlayFirstUnlocked() {
        Play(game.AvailableLevelsInUnlockOrder().First());
    }

    public void PlayLastUnlocked() {
        Play(game.AvailableLevelsInUnlockOrder().Last());
    }

    public void Play(Level level) {
        Application.LoadLevel(level.name);
    }

    public void NewGame() {
        game = new Game();
        Play(game.levels.First());
    }

    public void Map() {
        UI.instance.Map();
    }

    void Update() {
        if (Input.GetButtonDown("Map")) {
            Map();
        }
        if (UI.instance.currentScreen == UI.instance.map) {
            if (Input.GetButtonDown("Replay")) {
                Replay();
            }
            if (Input.GetButtonDown("New Game")) {
                NewGame();
            }
            if (Input.GetButtonDown("Play First Unlocked")) {
                PlayFirstUnlocked();
            }
            if (Input.GetButtonDown("Play Last Unlocked")) {
                PlayLastUnlocked();
            }
        }
    }
}