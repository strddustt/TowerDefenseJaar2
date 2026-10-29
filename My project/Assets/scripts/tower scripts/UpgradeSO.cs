using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New upgrade", menuName = "TD/Upgrade")]
public class UpgradeSO : ScriptableObject
{
    [SerializeField] private List<UpgradeStats> upgrades = new List<UpgradeStats>();
    public IReadOnlyList<UpgradeStats> Upgrades => upgrades;
    [SerializeField] private string description;
    public string Description => description;
}


[System.Serializable]
public class UpgradeStats
{
    public int damage;
    public float attackSpeed;
    public float range;
    public float footprint;
    public float cost;
    public MonoBehaviour ability;
    public string name;
}
