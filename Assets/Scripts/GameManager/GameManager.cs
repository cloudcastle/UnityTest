using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static bool firstAwake = true;

    static InstanceData instanceData = new InstanceData();

    static Game sample = new Game();
    public static Game game = new Game();

    public static GameManager instance;

    public void Awake() {
        instance = this;
        Load();

        if (firstAwake) {
            firstAwake = false;

            UpdateGameVersion();

            if (!Application.isEditor) {
                Play(game.currentLevel);
            }
        }
    }

    public Level CurrentLevel() {
        return game.levels.FirstOrDefault(level => level.name == SceneManager.GetActiveScene().name);
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
        SceneManager.LoadScene(level.name);
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

    void UpdateList<T>(List<T> target, List<T> sample, Func<T,T,bool> equals, Func<T, T> converter) {
        target.AddRange(sample.Where(sampleElement => !target.Any(targetElement => equals(targetElement, sampleElement))).Select(converter));
        target.RemoveAll(targetElement => !sample.Any(sampleElement => equals(targetElement, sampleElement)));
    }

    private void UpdateGameVersion() {
        UpdateList(game.levels, sample.levels, (x,y)=>x.name == y.name, x => new Level(x.name));
        game.levels.ForEach(gameLevel => {
            UpdateList(
                gameLevel.dependencies, 
                sample.levels.First(sampleLevel => sampleLevel.name == gameLevel.name).dependencies, 
                (x, y) => x.name == y.name, 
                x => game.levels.First(l => l.name == x.name)
            );
        });
        game.completedLevels.RemoveAll(l => !game.levels.Contains(l));
        if (!game.levels.Contains(game.currentLevel)) {
            game.currentLevel = game.AvailableLevelsInUnlockOrder().First();
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