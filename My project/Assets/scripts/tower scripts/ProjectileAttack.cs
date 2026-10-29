using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileAttack : AttackTypes
{
    private Projectile[] projectilesArray;
    private Queue<Projectile> projectiles = new Queue<Projectile>();
    private TowerRuntimeStats towerStats;
    private GameObject targetReference;
    private bool isLastHit;
    private Vector2[] waypoints;
    private TowerAttackInit targetList;
    private float projectileRadius;
    
    [SerializeField] private GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        projectileRadius = projectiles.Peek().GetComponent<CircleCollider2D>().radius;
        towerStats = GetComponent<TowerRuntimeStats>();
        targetList = GetComponent<TowerAttackInit>();
        waypoints = OnLoadManager.GetWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override IEnumerator Attack(float cooldown, GameObject target, int damage, TowerAttackInit tower)
    {
        if (isLastHit && targetReference == target)
        {
            targetList.AddIgnore(target.GetComponent<EnemyRuntimeStats>());
            yield break;
        }
        else
        {
            isLastHit = false;
        }
        if (projectiles.Count == 0)
        {
            Debug.LogWarning("projectile pool too small");
            yield break;
        }

        Projectile projectile = projectiles.Peek();
        EnemyRuntimeStats stats = target.GetComponent<EnemyRuntimeStats>();
        Vector2 prediction = PredictTarget.calculate(stats, projectile.Speed, this.transform.position);
        if (stats.isDoomed)
        {
            yield break;
        }
        else if (towerStats.damage > stats.hp && !RaycastButNotReally.CanMiss(transform.position, prediction, stats.hitboxSize, projectileRadius))
        {
            stats.SetDoomed();
        }

        //calculation to prevent the tower from hitting far outside its range
        int index = stats.currentWaypoint;
        float calculatedPercentage = CalculateProgressPoint.Calculate(prediction, index);
        while (calculatedPercentage >= 1 && index < waypoints.Length)
        {
            index++;
            calculatedPercentage = CalculateProgressPoint.Calculate(prediction, index);
        }
        var ranges = tower != null ? tower.PassRanges(index) : null;
        if (index < waypoints.Length && ranges != null && calculatedPercentage > ranges[1])
        {
            isLastHit = true;
            targetReference = target;
        }
        else if (waypoints.Length == index)
        {
            yield break;
        }

        projectiles.Dequeue();
        projectile.gameObject.SetActive(true);  
        projectile.Init(damage, prediction, gameObject);
        projectile.projectileHit += ProjectileHit;
        yield return new WaitForSeconds(cooldown);
    }
    private void ProjectileHit(Projectile projectile)
    {
        projectiles.Enqueue(projectile);
        projectile.projectileHit -= ProjectileHit;
        projectile.gameObject.transform.position = transform.position;
        projectile.gameObject.SetActive(false);
    }
    public void EnqueueProjectile(Projectile projectile)
    {
        projectiles.Enqueue(projectile);
    }
}
