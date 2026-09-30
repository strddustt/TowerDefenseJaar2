using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileAttack : AttackTypes
{
    private Projectile[] projectilesArray;
    private Queue<Projectile> projectiles = new Queue<Projectile>();
    [SerializeField] private GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        projectilesArray = GetComponentsInChildren<Projectile>(true);
        for (int i = 0; i < projectilesArray.Length; i++ )
        {
            projectiles.Enqueue(projectilesArray[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override IEnumerator Attack(float cooldown, GameObject target, int damage)
    {

        Projectile projectile = projectiles.Dequeue();
        EnemyRuntimeStats stats = target.GetComponent<EnemyRuntimeStats>();
        projectile.gameObject.SetActive(true);
        projectile.Init(damage, PredictTarget.calculate(stats, projectile.Speed, this.transform.position));
        projectile.projectileHit += ProjectileHit;
        yield return new WaitForSeconds(cooldown);
        if (projectiles.Count == 0)
        {
            Debug.LogWarning("projectile pool too small");
            yield break;
        }
    }
    private void ProjectileHit(Projectile projectile)
    {
        projectiles.Enqueue(projectile);
        projectile.projectileHit -= ProjectileHit;
        projectile.gameObject.transform.position = transform.position;
        projectile.gameObject.SetActive(false);
    }
}
