using System.Collections.Generic;
using System.Linq;
using System;

public class LevelComparerByCompletion : IComparer<Level>
{
    public int Compare(Level x, Level y) {
        return x.CompletionOrder().CompareTo(y.CompletionOrder());
    }
}