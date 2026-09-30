using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour, IDamageable
{
    public static event Action<int> onHit;
    private EnemyRuntimeStats stats;
    private bool isDead = false;
    private void Awake()
    {
        stats = GetComponent<EnemyRuntimeStats>();
    }
    public void TakeHit(HitData data)
    {
        int oldHp = stats.hp;
        int newHp = stats.hp;
        newHp -= data.damage;
        if (newHp <= 0) { newHp = 0; isDead = true; }
        stats.SetHp(newHp);
        onHit?.Invoke(oldHp - newHp);
    }
    private void LateUpdate()
    {
        if (isDead) { Destroy(gameObject); }
    }
}
