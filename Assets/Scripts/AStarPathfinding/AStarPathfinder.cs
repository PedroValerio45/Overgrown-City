using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder
{
    public static List<Waypoint> FindPath(Waypoint start, Waypoint goal)
    {
        var openSet = new PriorityQueue<Waypoint>();
        var cameFrom = new Dictionary<Waypoint, Waypoint>();
        var gScore = new Dictionary<Waypoint, float>();
        var fScore = new Dictionary<Waypoint, float>();

        foreach (var waypoint in Waypoint.AllWaypoints)
        {
            gScore[waypoint] = float.PositiveInfinity;
            fScore[waypoint] = float.PositiveInfinity;
        }

        gScore[start] = 0;
        fScore[start] = Vector3.Distance(start.transform.position, goal.transform.position);
        openSet.Enqueue(start, fScore[start]);

        while (openSet.Count > 0)
        {
            Waypoint current = openSet.Dequeue();

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            foreach (Waypoint neighbor in current.neighbors)
            {
                float tentativeGScore = gScore[current] + Vector3.Distance(current.transform.position, neighbor.transform.position);

                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Vector3.Distance(neighbor.transform.position, goal.transform.position);

                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                }
            }
        }

        return null;
    }

    static List<Waypoint> ReconstructPath(Dictionary<Waypoint, Waypoint> cameFrom, Waypoint current)
    {
        List<Waypoint> path = new List<Waypoint> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }
}
