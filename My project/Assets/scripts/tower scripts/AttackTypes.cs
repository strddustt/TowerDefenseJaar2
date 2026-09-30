using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//a bit convoluted for the scope, but better safe than sorry
public abstract class AttackTypes : MonoBehaviour
{
    public bool isDone { get; private set; } //exists for potential external active attack checks, e.g. for a target switch script on the tower
    private void Awake()
    {
        isDone = true;
    }
    public IEnumerator DoAttack(float cooldown, GameObject target, int damage)
    {
        isDone = false;
        yield return StartCoroutine(Attack(cooldown, target, damage));
        isDone = true;
    }
    protected abstract IEnumerator Attack(float cooldown, GameObject target, int damage);
}
