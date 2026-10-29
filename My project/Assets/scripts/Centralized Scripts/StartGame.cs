using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public static event Action start;

    public void onPress()
    {
        start?.Invoke();
        this.gameObject.SetActive(false);
    }
}
