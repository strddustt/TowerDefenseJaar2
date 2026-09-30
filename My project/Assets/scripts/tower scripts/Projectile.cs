using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    private bool isSet = false;
    private bool scheduleDestruction = false;
    public event Action<Projectile> projectileHit; //events pmo
    [SerializeField] private float speed = 3;
    public float Speed { get => speed; private set => speed = value; }
    HitData data = new HitData();
    
    private Vector2 target;
    
    public void Init(int damage, Vector2 target)
    {
        if (isSet && target != this.target) { Debug.LogWarning("target set more than once"); return; }
        isSet = true;
        data.damage = damage;
        this.target = target;
        data.tower = transform.parent.gameObject;
    }

    void Start()
    {
        
    }


    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
        if ((Vector2)transform.position == target)
        {
            scheduleDestruction = true;
        }
    }
    private void LateUpdate()
    {
        if (scheduleDestruction) { EndLifeCycle(); }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeHit(data);
            EndLifeCycle();
        }
    }
    public void EndLifeCycle()
    {
        scheduleDestruction = false;
        isSet = false;
        projectileHit?.Invoke(this);
    }
}
