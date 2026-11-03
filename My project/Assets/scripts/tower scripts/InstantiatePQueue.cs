using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    public void ExpandQueue()
    {
        GameObject current = Instantiate(prefab, transform);
        queueScript.EnqueueProjectile(current.GetComponent<Projectile>());
    }
}
