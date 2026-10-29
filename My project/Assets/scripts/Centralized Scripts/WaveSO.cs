using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "TD/wave")]
public class WaveSO : ScriptableObject
{
    [SerializeField, HideInInspector] public List<SpawnValues> values;
    [SerializeField, HideInInspector] public int waveReward;
    [SerializeField, HideInInspector] public int time;
}

[System.Serializable]
public struct SpawnValues
{
    public GameObject enemyType;
    [Min(1)] public int count;
    [Min(0)] public int delay;
    [Min(0.01f)] public float interval;
}
