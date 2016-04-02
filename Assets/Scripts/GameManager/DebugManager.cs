using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class DebugManager : MonoBehaviour
{
    public List<Level> levels;
    public List<Level> availableLevels;
    public List<Level> completedLevels;

    public List<string> levelsUnlockOrders;

    void Update() {
        levels = GameManager.game.levels;
        availableLevels = GameManager.game.AvailableLevelsInUnlockOrder();
        completedLevels = GameManager.game.levels.Where(l => l.Completed()).ToList();

        levelsUnlockOrders = GameManager.game.levels.Select(level => String.Format("{0} unlocked at {1}", level, level.UnlockOrder())).ToList();
    }
}