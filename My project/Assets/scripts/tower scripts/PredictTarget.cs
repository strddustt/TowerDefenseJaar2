using UnityEngine;

public static class PredictTarget
{
    /// <summary>
    /// speed is sampled upon firing. speed ups and slowdowns may cause misses from time to time. that's intended. if an enemy is presumed to reach the end of the path before the projectile can reach it, then the tower fires a hail mary (projectile can get destroyed if it preempts. 
    /// with 10 passes, the error rate is relatively low already, so-
    /// unless the tower has an exceptionally low bullet velocity, there won't really be super wrong guesses. if they do, then that's something you gotta deal with and can't rely on it guessing it perfectly in high stakes situations. or it has AoE and it doesn't matter anyway
    /// if the target moves more than half their hitbox size in a frame, this script will miss relatively consistently. don't make that happen. it's hard to make that happen anyway. 
    /// if the enemy is faster than the projectile, inconsistencies may arise in targeting. if that is what you're trying to debug, you might wanna look into that. i assumed that that wouldn't be an issue for the time being at least
    /// </summary>
    /// <param name="eStats"></param>
    /// <param name="pSpeed"></param>
    /// <param name="tPos"></param>
    /// <returns></returns>
    public static Vector2 calculate(EnemyRuntimeStats eStats, float pSpeed, Vector2 tPos)
    {
        Vector2[] waypoints = OnLoadManager.GetWaypoints();
        Vector2 trueEnemyPos = eStats.transform.position;
        float eSpeed = eStats.speed;
        Vector2 projectedPos = trueEnemyPos; // first guess
        bool isPastEnd = false;

        for (int pass = 0; pass < 10; pass++)
        {
            int index = eStats.currentWaypoint;
            float travelTime = Vector2.Distance(projectedPos, tPos) / pSpeed;
            float travelDistance = travelTime * eSpeed;

            Vector2 nDirection = (waypoints[index + 1] - waypoints[index]).normalized;
            projectedPos = trueEnemyPos + nDirection * travelDistance;
            float distanceToWaypoint = (waypoints[index + 1] - trueEnemyPos).magnitude;

            for (index = eStats.currentWaypoint + 1;
                 distanceToWaypoint < travelDistance && index + 1 < waypoints.Length;
                 index++)
            {

                float overshoot = travelDistance - distanceToWaypoint;
                Vector2 newDirection = waypoints[index + 1] - waypoints[index];
                projectedPos = waypoints[index] + newDirection.normalized * overshoot;
                distanceToWaypoint += newDirection.magnitude;
            }
            isPastEnd = travelDistance > distanceToWaypoint;
        }
        if (!isPastEnd)
            return projectedPos;
        else 
            return waypoints[waypoints.Length - 1];
    }
}