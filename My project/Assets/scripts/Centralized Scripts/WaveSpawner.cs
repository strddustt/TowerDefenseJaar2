using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private Coroutine runCheck;
    [SerializeField, Tooltip("Don't use this to add waves unless a desync or wipe happened")] private List<WaveSO> waves = new List<WaveSO>();
    public static event Action stoppedSpawning;
    private bool isSkipped = false;
    private int activeSegments = 0;
    public int ActiveSegments { get { return activeSegments; } private set { activeSegments = value; } }
    private int currentWave = 0;

    private void OnEnable()
    {
        StartGame.start += StartTimer;
        SkipButton.skipped += Skip;
    }

    private void StartTimer()
    {
        if (runCheck == null)
        {
            runCheck = StartCoroutine(Timer());
        }
        else
        {
            Debug.Log("tried starting waves again");
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(3);
        for (; currentWave < waves.Count; currentWave++)
        {
            yield return StartCoroutine(SpawnWaves(currentWave));
        }
    }

    private IEnumerator SpawnWaves(int index)
    {
        isSkipped = false;
        foreach (SpawnValues values in waves[index].values)
        {
            activeSegments++;
            StartCoroutine(SpawnSegment(values)); 
        }
        float timer = 0;
        while (timer < waves[index].time)
        {
            timer += Time.deltaTime;
            yield return null;
            if (isSkipped)
            {
                isSkipped = false;
                timer = waves[index].time;
            }
            
        }
        Cah.AddCash(waves[index].waveReward);

    }

    private IEnumerator SpawnSegment(SpawnValues values)
    {
        yield return new WaitForSeconds(values.delay);
        for (int i = 0; i < values.count; i++)
        {
            GameObject enemy = Instantiate(values.enemyType, transform.position, Quaternion.identity);
            EnemyList.AddEnemy(enemy, 0);
            if (i == values.count)
            {
                break;
            }
            yield return new WaitForSeconds(values.interval);
        }
        activeSegments--;   
        if (activeSegments == 0)
        {
            stoppedSpawning?.Invoke();
        }
    }


    private void Skip()
    {
        isSkipped = true;
    }


    public WaveSO GetWaveFromIndex(int index)
    {
        if (index < waves.Count)
            return waves[index];
        else
            Debug.Log("index not in bounds");
        return null;
    }
    public bool CanSkip()
    {
        if (activeSegments == 0)
            return true;
        else
            return false;

    }
    private void OnDisable()
    {
        runCheck = null;
        StartGame.start -= StartTimer;
        SkipButton.skipped -= Skip;
    }
}
