using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(menuName = "TD/towerstats")]
public class TowerStats : ScriptableObject
{
    public TargetType targetType {  get; private set; }

    [SerializeField] private float attackSpeed;
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value;  }
    [SerializeField] private int damage;
    public int Damage { get => damage; set => damage = value;  }
    [SerializeField] private float range;
    public float Range { get => range; set => range = value; }
}
