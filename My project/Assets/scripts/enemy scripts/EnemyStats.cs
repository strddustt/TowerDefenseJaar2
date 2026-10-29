using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TD/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] private int maxHp;
    public int MaxHp { get { return maxHp; } private set => maxHp = value;  }
    [SerializeField] private float speed;
    public float Speed { get { return speed; } private set => speed = value; }
    [SerializeField] private float hitboxSize;
    public float HitboxSize { get { return hitboxSize; } private set => hitboxSize = value; }
}
