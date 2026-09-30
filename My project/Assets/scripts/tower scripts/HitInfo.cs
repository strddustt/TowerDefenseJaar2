using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitData
{
    public int damage;
    public GameObject tower;
}
public interface IDamageable
{
    void TakeHit(HitData data);
}
