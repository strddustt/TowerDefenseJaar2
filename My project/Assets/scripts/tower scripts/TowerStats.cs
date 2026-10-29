using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(menuName = "TD/towerstats")]
public class TowerStats : ScriptableObject
{
    public TargetType targetType {  get; private set; }

    [SerializeField] private float attackSpeed;
    public float AttackSpeed { get => attackSpeed;}
    [SerializeField] private int damage;
    public int Damage { get => damage;}
    [SerializeField] private float range;
    public float Range { get => range;}
    [SerializeField] private float footprintRadius;
    public float FootprintRadius { get => footprintRadius;}
}
