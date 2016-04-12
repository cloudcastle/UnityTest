using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class MapScreen : UIScreen {
    public GameObject levelButtonSample;
    public Transform levelList;
    public ScrollRect scroll;
    public GameObject canScrollUp;
    public GameObject canScrollDown;

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

    void SetLevelList(List<Level> levels) {
        levels.ForEach(AddButton);
    }

    public void UpdateLevelList() {
        Clear();
        if (Cheats.on) {
            SetLevelList(GameManager.game.levels);
        } else {
            var completedLevels = GameManager.game.completedLevels;
            var unlockedLevels = GameManager.game.AvailableLevelsInReverseUnlockOrder();
            SetLevelList(completedLevels.Concat(unlockedLevels).ToList());
            scroll.verticalNormalizedPosition = 1f * unlockedLevels.Count / (completedLevels.Count + unlockedLevels.Count);
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

    public void OnUpdatedScrollPosition() {
        canScrollDown.SetActive(scroll.verticalNormalizedPosition > 0.01f);
        canScrollUp.SetActive(scroll.verticalNormalizedPosition < 0.99f);
    }
}
