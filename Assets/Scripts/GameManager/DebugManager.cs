using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class DebugManager : MonoBehaviour
{
    public List<string> levels;
    public List<string> availableLevels;
    public List<string> completedLevels;

    void Update() {
        levels = GameManager.game.levels.Select(l => l.name).ToList();
        availableLevels = GameManager.game.levels.Where(l => l.Unlocked()).Select(l => l.name).ToList();
        completedLevels = GameManager.game.levels.Where(l => l.Completed()).Select(l => l.name).ToList();
    }
}