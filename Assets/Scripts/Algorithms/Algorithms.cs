using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Algorithms
{
    public static void Dfs<StateType>(HashSet<StateType> reachedStates, StateType start, Func<StateType, IEnumerable<StateType>> getNextStates) {
        if (reachedStates.Contains(start)) {
            return;
        }
        reachedStates.Add(start);
        getNextStates(start).ToList().ForEach(nextState => {
            Dfs(reachedStates, nextState, getNextStates);
        });
    }

    public static HashSet<StateType> Reachable<StateType>(StateType start, Func<StateType, IEnumerable<StateType>> getNextStates, IEqualityComparer<StateType> comparer = null) {
        var result = new HashSet<StateType>(comparer ?? EqualityComparer<StateType>.Default);
        Dfs(result, start, getNextStates);
        return result;
    }
}
