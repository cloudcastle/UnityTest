using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class ItemTracker : ValueTracker<Item>
{
    public ItemTracker(Action<Item> setValue, Func<Item> getValue)
        : base(setValue, getValue) {
    }
}