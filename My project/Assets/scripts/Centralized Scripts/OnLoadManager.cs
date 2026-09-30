using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class OnLoadManager : MonoBehaviour
{
    [SerializeField] private Waypoints _waypoints;
    private static Vector2[] waypoints;
    public static Waypoints baseWaypoints { get; private set; }
    void Awake()
    {
        waypoints = _waypoints.waypoints.ToArray();
        EnemyList.Init();
    }
    public static Vector2[] GetWaypoints()
    {
        return waypoints; //returns a vector2 array

    }
        
}
