using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    public static List<EnemyStats>[] enemies { get; private set; }
    public static void Clear(int i) => enemies[i].Clear();
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics() => Array.ForEach(enemies, list => list.Clear());
    void Start()
    {
        enemies = new List<EnemyStats>[transform.childCount];
        for (int i = 0; i < enemies.Length; i++ )
        {
            enemies[i] = new List<EnemyStats>();
        }
    }
    public void AddEnemy (GameObject enemy, int index)
    {
        EnemyStats enemyStats= enemy.GetComponent<EnemyStats>();
        enemies[index].Add(enemyStats);
    }
    public void RemoveEnemy (GameObject enemy, int index)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        enemies[index].Remove(enemyStats);
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
                if (enemy.currentPercentage >= range.start && enemy.currentPercentage <= range.end)
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
    private static float GetMetric(EnemyStats e, TargetType type)
    {
        return type switch
        {
            TargetType.first or TargetType.last => e.currentPercentage,
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