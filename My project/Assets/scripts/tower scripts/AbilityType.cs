using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityType : ScriptableObject
{
    public abstract void Apply(TowerRuntimeStats stats);
}
