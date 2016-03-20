using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapScreen : UIScreen {
    public GameObject levelButtonSample;
    public Transform levelList;

    Pool levelButtonsPool;
    List<LevelButton> levelButtons = new List<LevelButton>();

    bool inited = false;
    void Init() {
        if (inited) {
            return;
        }
        inited = true;

        levelButtonsPool = new Pool(levelButtonSample);
    }

    public void UpdateLevelList() {
        Clear();

        if (Cheats.on) {
            GameManager.game.levels.ForEach(AddButton);
        } else {
            var unlockedLevels = GameManager.game.levels.Where(level => level.Unlocked() && !level.completed).ToList();
            unlockedLevels.ForEach(AddButton);
        }
    }

    public override void Show() {
        Init();
        base.Show();

        UpdateLevelList();
    }

    private void Clear() {
        levelButtons.ForEach(lb => lb.GetComponent<Poolable>().ReturnToPool());
        levelButtons.Clear();
    }

    void AddButton(Level level) {
        var button = levelButtonsPool.Take();
        button.transform.SetParent(levelList, worldPositionStays: false);
        var buttonScript = button.GetComponent<LevelButton>();
        buttonScript.SetLevel(level);
        levelButtons.Add(buttonScript);
    }
}
