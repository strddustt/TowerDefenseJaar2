using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Died : MonoBehaviour
{
    [SerializeField] private Canvas deathUI;
    [SerializeField] private Canvas UIToDisable;

    public void OnDeath()
    {
        UIToDisable.gameObject.SetActive(false);
        Instantiate(deathUI);
        Time.timeScale = 0;
    }
}
