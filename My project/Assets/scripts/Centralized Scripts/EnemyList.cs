using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    public static List<EnemyRuntimeStats>[] enemies { get; private set; }
    public static void Clear(int i) => enemies[i].Clear();
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        if (enemies != null)
        {
            Array.ForEach(enemies, list => list.Clear());
        }
    }

    public static void Init()
    {
        if (enemies == null)
        {
            enemies = new List<EnemyRuntimeStats>[OnLoadManager.GetWaypoints().Length];
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new List<EnemyRuntimeStats>();
            }
            Debug.Log(enemies.Length);
        }
    }
    public static void AddEnemy (GameObject enemy, int index)
    {
        EnemyRuntimeStats stats = enemy.GetComponent<EnemyRuntimeStats>();
        enemies[index].Add(stats);
    }
    public static void RemoveEnemy (GameObject enemy, int index)
    {
        EnemyRuntimeStats stats = enemy.GetComponent<EnemyRuntimeStats>();
        enemies[index].Remove(stats);
    }
    public static void SwitchIndex(GameObject enemy, int index)
    {
        EnemyRuntimeStats stats = enemy.GetComponent<EnemyRuntimeStats>();
        enemies[index - 1].Remove(stats);
        enemies[index].Add(stats);
    }
    /// <summary>
    /// this script checks sections of waypoints for enemies, only those given by a tower's listed visible indexes. for first, it iterates backwards, from the furthest point.
    /// breaks IMMEDIATELY upon finding the required enemy in a segment
    /// for last, the iteration is mirrored, the idea is similar.
    /// strongest and weakest do iterate over the entire list. if you find the tower targeting the wrong enemy, check first whether strongest or weakest works. if it does, the loop breaks when it shouldn't
    /// depends on the hardcoded "GetMetric" function, and doesn't handle random targeting yet. should be easy to code if need be.
    /// 
    /// ASSUMES THE SECTIONS IN THE LIST IN THE ARGUMENT ARE SORTED LOW TO HIGH
    /// </summary>
    /// <param name="type"></param>
    /// <param name="ranges"></param>
    /// <returns></returns>
    public static GameObject GetEnemy(TargetType type, List<indexData> ranges)
    {
        GameObject target = null;
        float best = 0f;
        bool wantMax = type == TargetType.first || type == TargetType.strongest;

        bool reverseIterate = type == TargetType.first;
        bool canBreakEarly = type == TargetType.first || type == TargetType.last;

        int startI = reverseIterate ? ranges.Count - 1 : 0;
        int endI = reverseIterate ? -1 : ranges.Count;
        int step = reverseIterate ? -1 : 1;


        for (int i = startI; i != endI; i += step)
        {
            var range = ranges[i];
            foreach (var enemy in enemies[range.index])
            {
                if (enemy.pathPercentage >= range.start && enemy.pathPercentage <= range.end)
                {
                    float metric = GetMetric(enemy, type);
                    bool better = target == null || (wantMax ? metric > best : metric < best);
                    if (better)
                    {
                        best = metric;
                        target = enemy.gameObject;
                    }
                }
            }

            if (target != null && canBreakEarly)
                break;
        }

        return target;
    }
    private static float GetMetric(EnemyRuntimeStats e, TargetType type)
    {
        return type switch
        {
            TargetType.first or TargetType.last => e.pathPercentage,
            TargetType.strongest or TargetType.weakest => e.hp,
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
public enum TargetType
{
    first,
    last,
    strongest,
    weakest,
    random,
}
public struct indexData
{
    public int index;
    public float start;
    public float end;
}