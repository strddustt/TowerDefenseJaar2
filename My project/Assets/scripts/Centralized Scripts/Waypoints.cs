using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TD/MapData")]
public class Waypoints : ScriptableObject
{
    [SerializeField] private Vector2[] actualWaypoints;
    public IReadOnlyList<Vector2> waypoints => actualWaypoints;

}
