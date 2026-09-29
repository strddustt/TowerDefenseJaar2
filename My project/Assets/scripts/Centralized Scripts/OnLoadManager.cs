using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLoadManager : MonoBehaviour
{
    [SerializeField] private Waypoints waypoints;
    void Awake()
    {
        DetectRange.FindWaypoints(waypoints);
    }
}
