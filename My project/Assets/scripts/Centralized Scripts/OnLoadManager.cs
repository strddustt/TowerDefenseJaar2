using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class OnLoadManager : MonoBehaviour
{
    [SerializeField] private Waypoints _waypoints;
    private static Vector2[] waypoints;
    [SerializeField] private static float pathhalfWidth = 0.5f;
    public static float pathHalfWidth { get { return pathhalfWidth; } private set { pathhalfWidth = value; } }
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
