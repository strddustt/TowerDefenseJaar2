using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerAttackInit : MonoBehaviour
{
    private AttackTypes attackLogic;
    private EnemyList enemyList;
    private Transform target;
    [SerializeField]
    private MonoBehaviour otherAttack;
    void Start()
    {
        attackLogic = GetComponent<AttackTypes>();
        enemyList = GetComponent<EnemyList>();
        //stats = GetComponent<TowerStats>();

    }
    public void GiveTarget(Transform target)
    {
        this.target = target.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            StartCoroutine(StartAttack());
        }
        else
        {
            
        }
    }
    private IEnumerator StartAttack()
    {
        yield return StartCoroutine(attackLogic.DoAttack());
    }
    

}