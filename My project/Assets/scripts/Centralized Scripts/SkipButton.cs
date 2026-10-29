using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipButton : MonoBehaviour
{
    public static event Action skipped;
    private void Start()
    {
        WaveSpawner.stoppedSpawning += ShowButton;
    }
    public void Skip()
    {
        skipped?.Invoke();
    }
    public void DontSkip()
    {
        
    }
    private void ShowButton()
    {

    }
}
