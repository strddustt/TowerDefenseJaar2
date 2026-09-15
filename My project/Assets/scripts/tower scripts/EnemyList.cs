using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyList : MonoBehaviour
{
    private TowerAttackInit attack;
    private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private GameObject target;
    //private DetectOvertake changeTarget;
    private EnemyStats stats;
    private void Start()
    {
        attack = GetComponent<TowerAttackInit>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            stats = collision.GetComponent<EnemyStats>();
           // changeTarget = collision.GetComponent<DetectOvertake>();
            //changeTarget.switchTarget += AlternateTarget;
            enemies.Add(collision.gameObject);
            ChooseTarget();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemies.Remove(collision.gameObject);
            if (enemies.Count > 0)
            {
                ChooseTarget();
            }
        }
    }
    public void SwitchEnemy()
    {
        stats = enemies[0].GetComponent<EnemyStats>();
        enemies.Remove(enemies[0]);
        ChooseTarget();
    }
    void ChooseTarget()
    {
        if (enemies.Count > 0)
        {
            target = enemies[0];
            // additional logic later based on enum
            attack.GiveTarget(target.transform);
        }
    }
    public void AlternateTarget(GameObject targetcheck, GameObject newtarget)
    {
        if (targetcheck == enemies[0])
        {
            enemies.Remove(newtarget);
            enemies.Insert(0, newtarget);
        }
    }
}