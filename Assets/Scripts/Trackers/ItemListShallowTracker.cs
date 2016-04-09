using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class ItemListShallowTracker : ListShallowTracker<Item>
{
    public ItemListShallowTracker(Action<List<Item>> setList, Func<List<Item>> getList)
        : base(setList, getList) {
    }
}