using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStats : MonoBehaviour
{
    public TargetType targetType {  get; private set; }
    
    public float attackSpeed {  get; private set; }
    public float damage { get; private set; }
    public float range 
    {
        get => range;
        private set
        {
            range = value;
        }
    }
    private void Start()
    {
        targetType = TargetType.first;
    }
}
