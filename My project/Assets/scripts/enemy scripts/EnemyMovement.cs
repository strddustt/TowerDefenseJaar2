using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyRuntimeStats stats;
    private Vector2[] waypoints;
    private int index = 0;
    Vector2 path;
    
    void Start()
    {
        waypoints = OnLoadManager.GetWaypoints();
        stats = GetComponent<EnemyRuntimeStats>();
        path = waypoints[index + 1] - waypoints[index];
        
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[index + 1], stats.speed * Time.deltaTime);
        float progress = Vector2.Dot((Vector2)transform.position - waypoints[index], path) / path.sqrMagnitude;
        progress = Mathf.Clamp01(progress);
        stats.SetPercentage(progress);
        if ((Vector2)transform.position == waypoints[index+1])
            SwitchSegment();
    }
    private void SwitchSegment()
    {
        index++;
        stats.ChangeSegment(true);
        stats.SetPercentage(0f);
        EnemyList.SwitchIndex(gameObject, index);
        if (index + 1 >= waypoints.Length)
        {
            enabled = false; // stop Update from running while waiting for destruction
            return;
        }
        path = waypoints[index + 1] - waypoints[index];
    }
}
