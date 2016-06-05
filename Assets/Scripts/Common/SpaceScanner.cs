using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public static class SpaceScanner
{
    const int MAX_RESULTS_COUNT = 1000;
    public static int overlapCount;
    public static Collider[] overlapResults = new Collider[MAX_RESULTS_COUNT];
}