using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    static bool firstAwake = true;

    static InstanceData instanceData = new InstanceData();

    public static Game game = new Game();

    public static GameManager instance;

    public void Awake() {
        instance = this;
        Load();

        if (firstAwake) {
            firstAwake = false;
            Play(game.currentLevel);
        }
    }

    public Level CurrentLevel() {
        return game.levels.FirstOrDefault(level => level.name == Application.loadedLevelName);
    }

    public void CompleteLevel() {
        var level = CurrentLevel();
        game.completedLevels.Add(level);
        Save();
        UI.instance.CompletionScreen();
    }

    public void Pause() {
        UI.instance.PauseScreen();
    }

    public void Resume() {
        UI.instance.Game();
    }

    public void Restart() {
        UI.instance.Confirm(() => {
            Play(CurrentLevel());
        }, "Restart");
    }

    public void Play(Level level) {
        Debug.Log(string.Format("Play {0}", level));
        game.currentLevel = level;
        Save();
        Application.LoadLevel(level.name);
    }

    public void PlayLastUnlocked() {
        Play(game.AvailableLevelsInReverseUnlockOrder().First());
    }

    public void NewGame() {
        game = new Game();
        Play(game.levels.First());
    }

    public void Map() {
        UI.instance.Confirm(() => {
            UI.instance.Map();
        }, "Map");
    }

    public void MapNoConfirm() {
        UI.instance.Map();
    }

    public void Options() {
        UI.instance.OptionsScreen();
    }

    public void Save() {
        FileManager.SaveToFile(game, instanceData.currentGame);
    }

    public void Load() {
        game = FileManager.LoadFromFile<Game>(instanceData.currentGame);
        if (game == null) {
            Debug.Log("Game not detected. Creating new");
            game = new Game();
        }
    }

    void Update() {
        if (UI.instance.CurrentScreen == null) {
            if (Input.GetButtonDown("Pause")) {
                Pause();
            }
            if (Input.GetButtonDown("Map")) {
                Map();
            }
            if (Input.GetButtonDown("Restart")) {
                Restart();
            }
        }
    }
}