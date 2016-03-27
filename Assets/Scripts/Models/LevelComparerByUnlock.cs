using System.Collections.Generic;
using System.Linq;
using System;

public class LevelComparerByUnlock : IComparer<Level>
{
    public int Compare(Level x, Level y) {
        int unlockOrderComparison = x.UnlockOrder().CompareTo(y.UnlockOrder());
        if (unlockOrderComparison != 0) {
            return unlockOrderComparison;
        }
        int gameOrderComparison = x.GameOrder().CompareTo(y.GameOrder());
        return gameOrderComparison;
    }
}