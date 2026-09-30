using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TowerRuntimeStats : MonoBehaviour
{
    [SerializeField] private TowerStats stats;
    public int damage { get; private set; }
    public float attackSpeed { get; private set;  }
    public float range { get; private set; }
    public TargetType targetType { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        damage = stats.Damage;
        attackSpeed = stats.AttackSpeed;
        range = stats.Range;
        targetType = stats.targetType;
        
    }

    // Update is called once per frame
    public void SetDamage(int value)
    {
        damage = value;
    }
    public void SetAttackSpeed(float value)
    {
        attackSpeed = value;
    }
    public void SetRange(float value)
    {
        Mathf.Clamp(0.1f, value, 100);
        range = value;
    }
    public void SetTargeting(TargetType type)
    {
        targetType = type;
    }
}
