using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] public int hp {  get; private set; }
    [SerializeField] public int maxHp { get; private set; }
    [SerializeField] public float speed { get; private set; }
}
