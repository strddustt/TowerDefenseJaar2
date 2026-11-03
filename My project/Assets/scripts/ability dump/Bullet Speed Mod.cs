using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BSM", menuName = "TD/Abilities/Bullet Speed Mod")]
public class BSM : AbilityType
{
    [SerializeField] private float bulletSpeed = 3f;
    public override void Apply(TowerRuntimeStats stats)
    {
        BulletSpeedMod mod = stats.gameObject.AddComponent<BulletSpeedMod>();
        mod.Setup(bulletSpeed);
    }
}
public class BulletSpeedMod : MonoBehaviour
{
    public void Setup(float speed)
    {
        Projectile[] projectiles = GetComponentsInChildren<Projectile>();
        foreach( Projectile projectile in projectiles)
        {
            projectile.SetSpeed(projectile.Speed + speed);
        }
        Destroy(this);
    }
}
