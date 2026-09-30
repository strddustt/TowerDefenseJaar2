using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerAttackInit : MonoBehaviour
{
    private AttackTypes attackLogic;
    private readonly List<indexData> sightlines = new List<indexData>();
    [SerializeField] private GameObject target; // DO NOT FORGET TO HIDE AGAIN LATER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private TowerRuntimeStats stats;
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
    }
    private IEnumerator StartAttack()
    {
        Debug.Log("found enemy");
        yield return StartCoroutine(attackLogic.DoAttack(stats.attackSpeed, target, stats.damage));
    }
    private void FindTarget()
    {
        DetectRange.FindOverlap(transform.position, stats.range, sightlines);
        target = EnemyList.GetEnemy(stats.targetType, sightlines);
        if (target == null)
        {
            return;
        }
            StartCoroutine(StartAttack());
    }

}