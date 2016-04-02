using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class UnitySerializableLevel
{
    public string name;

    public override string ToString() {
        return name;
    }
}