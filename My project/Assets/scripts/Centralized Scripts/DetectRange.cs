using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public static class DetectRange
{
    private static Vector2[] waypoints;
    public static void FindWaypoints(Waypoints points)
    {
        waypoints = points.waypoints;
    }
    

    /// <summary>
    /// simple dot -> leg -> normalized t calculation. doesn't actually solve for the vectors of the overlap points, only their % along the path expressed as a decimal value between 0 and 1 (clamped), named t_a & t_b.
    /// outputs everything into a list starting from the first waypoint, and that list can then be passed to enemylist's GetEnemy to retrieve a target. 
    /// does not store segments it cannot see at all, also doesn't work if the circumference just barely grazes the path, because that'd be annoying to manage and would not be noticeable if it happened
    /// !!!!!!!make sure to actually call if towerstats.range ever changes. since range is a private set, just include a refresh of this function in the function you'll inevitably use for the private set!!!!!!!!!
    /// !!!!!!!!!! TARGETING WILL NOT WORK IF YOU DON'T !!!!!!!!!!!!!!!
    /// </summary>
    public static List<indexData> FindOverlap(Vector2 position, float range)
    {
        List<indexData> indexes = new List<indexData>();
        int i = 1;
        foreach (var waypoint in waypoints)
        {
            if (waypoints.Length <= i)
            {
                continue;
            }
            Vector2 direction = waypoints[i] - waypoint;
            Vector2 distance = position - (Vector2)waypoint;
            float t = (direction.x * distance.x + direction.y * distance.y) / (direction.x * direction.x + direction.y * direction.y);
            Vector2 closest = (Vector2)waypoint + t * direction;
            Vector2 distanceCircle = closest - (Vector2)position;
            float distanceMagnitude = Mathf.Sqrt(distanceCircle.x * distanceCircle.x + distanceCircle.y * distanceCircle.y);
            if (distanceMagnitude < range) // if this is not true, there will be either 1 or 0 t, which is worthless for the list, and it just means that the path is out of range
            {
                float leg = Mathf.Sqrt(range * range - distanceMagnitude * distanceMagnitude);
                float d = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
                float t_a = t - leg / d;
                float t_b = t + leg / d;
                if (t_a > 1 || t_b < 0)
                {
                    i++;
                    continue;
                }
                else
                {
                    t_a = MathF.Max(0f, t_a);
                    t_b = MathF.Min(1f, t_b);
                    indexData newindex = new indexData();
                    newindex.start = t_a;
                    newindex.end = t_b;
                    newindex.index = i - 1;
                    indexes.Add(newindex);

                }
                
            }
            i++;

        }
        return indexes;
    }
}
