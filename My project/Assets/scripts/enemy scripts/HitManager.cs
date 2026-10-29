using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour, IDamageable
{
    public static event Action<int> onHit;
    private DeathHandler deathScript;
    private EnemyRuntimeStats stats;
    private void Awake()
    {
        stats = GetComponent<EnemyRuntimeStats>();
        deathScript = GetComponent<DeathHandler>();
    }
    public void TakeHit(HitData data)
    {
        int oldHp = stats.hp;
        int newHp = stats.hp;
        newHp -= data.damage;
        stats.SetHp(newHp);
        onHit?.Invoke(oldHp - newHp);
    }

}
