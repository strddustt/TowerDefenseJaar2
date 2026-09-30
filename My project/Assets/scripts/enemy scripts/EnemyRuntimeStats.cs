using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRuntimeStats : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    public int currentWaypoint {  get; private set; }
    public int hp { get; private set; }
    public float pathPercentage { get; private set; }
    public float speed { get; private set; }
    private void Awake()
    {
        currentWaypoint = 0;
        hp = stats.MaxHp;
        pathPercentage = 0f;
        speed = stats.Speed;
        EnemyList.AddEnemy(this.gameObject, currentWaypoint);
    }

    public void SetHp(int value)
    {
        hp = Mathf.Clamp(value, 0 , stats.MaxHp);
    }
    public void SetPercentage(float value)
    {
        pathPercentage = value;
    }
    public void ChangeSegment(bool increase)
    {
        if (increase)
            currentWaypoint++;
        else
            currentWaypoint--;
    }
}
