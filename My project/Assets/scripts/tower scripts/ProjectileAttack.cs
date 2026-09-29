using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileAttack : AttackTypes
{
    private Projectile[] projectiles;
    public int activeProjectiles = 0;
    [SerializeField] private GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        projectiles = GetComponentsInChildren<Projectile>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override IEnumerator Attack(float cooldown, Vector2 target)
    {
        projectiles[activeProjectiles].gameObject.SetActive(true);
        projectiles[activeProjectiles].target = target;
        projectiles[activeProjectiles].projectileHit += ProjectileHit;
        activeProjectiles++;
        yield return new WaitForSeconds(cooldown);
    }
    private void ProjectileHit(Projectile projectile)
    {
        activeProjectiles--;
        projectile.gameObject.transform.position = transform.position;
    }
}
