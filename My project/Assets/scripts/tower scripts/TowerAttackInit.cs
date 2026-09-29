using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerAttackInit : MonoBehaviour
{
    private AttackTypes attackLogic;
    [SerializeField] private Transform target; // DO NOT FORGET TO HIDE AGAIN LATER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private TowerStats stats;
    private MonoBehaviour otherAttack;
    void Start()
    {
        attackLogic = GetComponent<AttackTypes>();
        stats = GetComponent<TowerStats>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null && attackLogic.isDone == true)
        {
            StartCoroutine(StartAttack());
        }
        else
        {
            
        }
    }
    private IEnumerator StartAttack()
    {
        EnemyList.GetEnemy(stats.targetType, DetectRange.FindOverlap(transform.position, stats.range));
        yield return StartCoroutine(attackLogic.DoAttack(stats.attackSpeed, target.position));
    }
    

}