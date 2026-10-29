using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyUpgrade : MonoBehaviour
{
    public void Apply(UpgradeStats upgrade, TowerRuntimeStats stats)
    {
        stats.SetDamage(upgrade.damage);
        stats.SetAttackSpeed(upgrade.attackSpeed);
        stats.SetRange(upgrade.range);
        stats.SetFootprint(upgrade.footprint);
        if (upgrade.ability != null)
        {
            stats.gameObject.AddComponent(upgrade.ability.GetType());
        }
    }
}

