using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public static List<Waypoint> AllWaypoints = new List<Waypoint>();
    public List<Waypoint> neighbors;

    private void Awake() => AllWaypoints.Add(this);
    private void OnDestroy() => AllWaypoints.Remove(this);

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.2f);

        if (neighbors == null) return;

        Gizmos.color = Color.cyan;
        foreach (var neighbor in neighbors)
        {
            if (neighbor != null)
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
#endif
}