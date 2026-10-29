using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlacementCheck : MonoBehaviour
{
    private bool colliding = false;
    private float radius;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<TowerRuntimeStats>(out _))
        {
            colliding = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<TowerRuntimeStats>(out _))
        {
            colliding = false;
        }
    }
    public bool CheckPlacement()
    {
        if (PathCheck.IsValid(transform.position, radius ) && !colliding)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetRadius(float value)
    {
        radius = value;
    }
}





public static class PathCheck
{

    public static bool IsValid(Vector2 pos, float towerRadius)
    {
        Vector2[] waypoints = OnLoadManager.GetWaypoints();
        float minDist = OnLoadManager.pathHalfWidth + towerRadius;

        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Vector2 a = waypoints[i];
            Vector2 b = waypoints[i + 1];
            
            if (DistanceToSegment(pos, a, b) < minDist)
                return false;
        }
        return true;
    }
    private static float DistanceToSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float t = Vector2.Dot(p - a, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t);
        return Vector2.Distance(p, a + ab * t);
    }
}
