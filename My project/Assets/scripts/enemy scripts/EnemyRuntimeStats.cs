using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyRuntimeStats : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    private DeathHandler deathHandler;
    public bool isDoomed { get; private set; }
    public int currentWaypoint {  get; private set; }
    private int _hp;
    public int hp
    {
        get { return _hp; }
        private set 
        { 
            _hp = value; 
            if (_hp == 0) CallDeath(); 
        }
    }
    public float hitboxSize => stats.HitboxSize;
    public float pathPercentage { get; private set; }
    public float speed { get; private set; }
    private void Start()
    {
        isDoomed = false;
        deathHandler = GetComponent<DeathHandler>();
        currentWaypoint = 0;
        hp = stats.MaxHp;
        pathPercentage = 0f;
        speed = stats.Speed;
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
    public void CallDeath()
    {
        deathHandler.die(currentWaypoint);
    }
    public void SetDoomed()
    {
        isDoomed = true;
    }
}
