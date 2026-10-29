using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;

public class TowerAttackInit : MonoBehaviour
{
    private AttackTypes attackLogic;
    private readonly List<indexData> sightlines = new List<indexData>();
    private readonly HashSet<EnemyRuntimeStats> ignoredTargets = new HashSet<EnemyRuntimeStats>();
    private GameObject target;
    private float targetExitPercentage;     
    private TowerRuntimeStats stats;
    private EnemyRuntimeStats enemyStats;
    private MonoBehaviour otherAttack;
    void Start()
    {
        attackLogic = GetComponent<AttackTypes>();
        stats = GetComponent<TowerRuntimeStats>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (attackLogic.isDone == true)
        {
            FindTarget();
        }
        else
        {
            
        }
        if (enemyStats == null)
            return;
        else if (targetExitPercentage <= enemyStats.pathPercentage)
        {
            target = null;
            targetExitPercentage = 0;
        }
    }
    private IEnumerator StartAttack()
    {
        yield return StartCoroutine(attackLogic.DoAttack(stats.attackSpeed, target, stats.damage, this));
    }
    private void FindTarget()
    {
        DetectRange.FindOverlap(transform.position, stats.range, sightlines);
        target = EnemyList.GetEnemy(stats.targetType, sightlines, ignoredTargets);
        if (target == null)
        {
            return;
        }
        else if (targetExitPercentage == 0)
        {
            enemyStats = target.GetComponent<EnemyRuntimeStats>();
            foreach (indexData indexData in sightlines)
            {
                if (indexData.index == enemyStats.currentWaypoint)
                {
                    targetExitPercentage = indexData.end;
                }
            }
        }

        StartCoroutine(StartAttack());
    }
    public float[] PassRanges(int index)
    {
        int trueindex = 0;
        for (int i = 0; i < sightlines.Count; i++)
        {
            if (index == sightlines[i].index)
                trueindex = i;
        }
        if (trueindex == 0)
            return null;
        float[] values = new float[2];
        values[0] = sightlines[trueindex].start;
        values[1] = sightlines[trueindex].end;
        return values;
    }
    public void AddIgnore(EnemyRuntimeStats target)
    {
        ignoredTargets.Add(target);
    }

}