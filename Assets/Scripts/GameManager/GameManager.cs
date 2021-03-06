﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public const string levelGraph = "Graph";

    static bool firstAwake = true;

    static InstanceData instanceData = new InstanceData();

    static Game sample = new Game();
    public static Game game = new Game();

    public static GameManager instance;

    public UnityEvent onLevelCompleted;

    public void Awake() {
        instance = this;
        Load();

        if (firstAwake) {
            firstAwake = false;

            UpdateGameVersion();
            Save();

            if (!Application.isEditor) {
                Play(game.currentLevel);
            }
        }
    }

    public Level CurrentLevel() {
        return game.levels.FirstOrDefault(level => level.name == SceneManager.GetActiveScene().name);
    }

    public bool OnLevel() {
        return CurrentLevel() != null;
    }

    public bool OnMap() {
        return SceneManager.GetActiveScene().name == levelGraph;
    }

    public void CompleteLevel() {
        if (OnLevel()) {
            var level = CurrentLevel();
            if (!level.Completed()) {
                game.completedLevels.Add(level);
            }
            Save();
        }
        LevelUI.instance.CompletionScreen();
        onLevelCompleted.Invoke();
    }

    public void Pause() {
        if (!OnMap()) {
            LevelUI.instance.PauseScreen();
        }
    }

    public void Resume() {
        LevelUI.instance.Game();
    }

    public void Restart(bool confirm = true) {
        if (CurrentLevel() != null) {
            if (confirm) {
                LevelUI.instance.Confirm(() => {
                    Play(CurrentLevel());
                }, "Restart");
            } else {
                Play(CurrentLevel());
            }
        } else {
            if (!OnMap()) {
                LevelUI.instance.Confirm(() => {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }, "Restart"); 
            }
        }
    }

    public void Play(Level level) {
        Debug.Log(string.Format("Play {0}", level));
        game.currentLevel = level;
        Save();
        if (level != null) {
            SceneManager.LoadScene(level.name);
        } else {
            SceneManager.LoadScene(levelGraph);
        }
    }

    public void PlayLastUnlocked() {
        Play(game.AvailableLevelsInReverseUnlockOrder().First());
    }

    public void NewGame() {
        UI.instance.Confirm(() => {
            game = new Game();
            Play(game.levels.First());
        }, "WIPE ALL PROGRESS???");
    }

    public void Map() {
        LevelUI.instance.Confirm(() => {
            LevelUI.instance.Map();
        }, "Map");
    }

    public void MapNoConfirm() {
        LevelUI.instance.Map();
    }

    public void Options() {
        LevelUI.instance.OptionsScreen();
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
        if (game.currentLevel != null && !game.levels.Contains(game.currentLevel)) {
            game.currentLevel = game.AvailableLevelsInUnlockOrder().First();
        }
        game.completedLevels = game.completedLevels.Distinct().ToList();

        if (game.levelGraphCameraZoom == 0) {
            game.levelGraphCameraZoom = 1;
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