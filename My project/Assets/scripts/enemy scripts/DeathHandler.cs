using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    public static event Action<GameObject, int> onDeath;
    private bool scheduledDeath;
    private void LateUpdate()
    {
        if (scheduledDeath)
        {
            scheduledDeath = false;
            Destroy(gameObject);
        }
    }
    public void die(int index)
    {
        onDeath?.Invoke(gameObject, index);
        scheduledDeath = true;
    }
}
