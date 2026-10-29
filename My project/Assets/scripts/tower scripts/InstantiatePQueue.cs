using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// intended to calculate and create the minimum needed projectiles for its max firerate + 10%, don't have the time for that yet though
/// obviously only attach this script if the tower has the relevant projectile attack logic
/// </summary>
public class InstantiatePQueue : MonoBehaviour
{
    private ProjectileAttack queueScript;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int tempQueueSize;
    // Start is called before the first frame update
    void Awake()
    {
        queueScript = GetComponent<ProjectileAttack>();
        for (int i  = 0; i < tempQueueSize; i++)
        {
            GameObject current = Instantiate(prefab, transform);
            queueScript.EnqueueProjectile(current.GetComponent<Projectile>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
