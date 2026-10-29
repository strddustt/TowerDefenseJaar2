using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    private bool isSet = false;
    private bool scheduleHitDestruction = false;
    private bool scheduleDestruction = false;
    public event Action<Projectile> projectileHit; //events pmo
    [SerializeField] private float speed = 3;
    public float Speed { get => speed; private set => speed = value; }
    HitData data = new HitData();
    
    private Vector2 target;
    private GameObject targetObject;
    
    public void Init(int damage, Vector2 target, GameObject targetObject)
    {
        if (isSet && target != this.target) { Debug.LogWarning("target set more than once"); return; }
        isSet = true;
        this.targetObject = targetObject;
        data.damage = damage;
        this.target = target;
        data.tower = transform.parent.gameObject;
        DeathHandler.onDeath += EnemyDied;
    }

    void Start()
    {
        
    }


    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
        if ((Vector2)transform.position == target)
        {
            scheduleHitDestruction = true;
        }
    }
    private void LateUpdate()
    {
        if (scheduleHitDestruction) { EndLifeCycle(true); }
        if (scheduleDestruction) { EndLifeCycle(false); }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeHit(data);
            scheduleHitDestruction = true;
        }
    }
    public void EndLifeCycle(bool hit)
    {
        scheduleHitDestruction = false;
        isSet = false;
        if (hit)
            projectileHit?.Invoke(this);
        else
            Destroy(gameObject);
    }
    private void EnemyDied(GameObject target, int index)
    {
        if (target == targetObject)
            scheduleDestruction = true;
        
    }
    private void OnDisable()
    {
        DeathHandler.onDeath -= EnemyDied;
    }
}
