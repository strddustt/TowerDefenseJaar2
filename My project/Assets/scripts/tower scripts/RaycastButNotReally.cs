using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class RaycastButNotReally
{

    public static bool CanMiss(Vector2 towerPos, Vector2 targetPos, float enemyRadius, float bulletRadius)
    {
        Vector2[] waypoints = OnLoadManager.GetWaypoints();
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Vector2 point;
            Vector2[] lineA = new Vector2[]{ towerPos, targetPos };
            Vector2[] lineB = new Vector2[] { waypoints[i], waypoints[i+1] };
            if (!LineIntersection(out point, lineA, lineB, true, enemyRadius + bulletRadius))
                continue;
            else if (point == default)
            {
                return true;
            }
            else
            {
                if (Vector2.Distance(point, targetPos) < 0.1)
                    continue;
                else
                    return true;

            }
        }
        return false;
    }

    private static float Cross(Vector2 v, Vector2 w) => v.x * w.y - v.y * w.x;

    private static bool LineIntersection(out Vector2 point, Vector2[] lineA, Vector2[] lineB, bool first, float R = 0)
    {
        
        Vector2 r = lineA[1] - lineA[0];
        Vector2 s = lineB[1] - lineB[0];
        Vector2 dir = r / r.magnitude;
        Vector2 n = new Vector2(-dir.y, dir.x);
        float denom = Cross(r, s);

        point = default;
        if (Mathf.Abs(denom) < 1e-6f) return false; // parallel or collinear

        float t = Cross(lineB[0] - lineA[0], s) / denom;
        float u = Cross(lineB[0] - lineA[0], r) / denom;
        float Rt = R / r.magnitude; //
        float Ru = R / s.magnitude;


        if (t < 0 - Rt || t > 1 + Rt || u < 0 - Ru || u > 1 + Ru) return false; // crossing is outside a segment
        else if (first)
        {
                Vector2[] linePlus = new Vector2[2] { (lineA[0] + R * n), (lineA[1] + R * n) };
                Vector2[] lineMinus = new Vector2[2] { (lineA[0] - R * n), (lineA[1] - R * n) };
                if (LineIntersection(out point, linePlus, lineB, false))
                {
                    if (Vector2.Distance(point, lineA[1]) >= R*1.2)
                        return true;
                }
                if (LineIntersection(out point, lineMinus, lineB, false))
                {
                    if (Vector2.Distance(point, lineA[1]) >= R * 1.2)
                        return true;
                }
        }
        point = lineA[0] + t * r;
        return true;
    }
}
    