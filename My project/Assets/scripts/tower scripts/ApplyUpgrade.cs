using UnityEngine;

public static class ApplyUpgrade
{
    public static void Apply(UpgradeStats upgrade, TowerRuntimeStats stats)
    {
        stats.SetDamage(stats.damage + upgrade.damage);
        stats.SetAttackSpeed(stats.attackSpeed - upgrade.attackSpeed);
        stats.SetRange(stats.range + upgrade.range);
        stats.SetFootprint(stats.footprintRadius + upgrade.footprint);

        if (upgrade.ability != null)
            upgrade.ability.Apply(stats);
    }
}