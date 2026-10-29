using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CalculateProgressPoint
{
    public static float Calculate(Vector2 point, int index)
    {
        Vector2[] waypoints = OnLoadManager.GetWaypoints();
        if (index >= waypoints.Length)
        {
            return 1;
        }
        Vector2 path = waypoints[index + 1] - waypoints[index];
        return Vector2.Dot(point - waypoints[index], path) / path.sqrMagnitude;
        
    }
}
