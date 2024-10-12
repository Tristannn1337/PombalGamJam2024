using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathfinding : MonoBehaviour
{
    [SerializeField] float waypointDistance = 5f;

    Seeker seeker;
    Path path;
    int currentWaypoint;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
    }

    public void CalculatePath(bool overridePath, Vector3 targetPosition)
    {
        if (overridePath || seeker.IsDone())
        {
            seeker.StartPath(transform.position, targetPosition, OnPathComplete);
        }
    }
    public void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            currentWaypoint = 0;
        }
        else
        {
            Debug.Log(path.errorLog);
        }
    }
    public bool PathExists()
    {
        return path != null;
    }
    public Vector2? GetNextWaypoint()
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            return null;
        }

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < waypointDistance)
        {
            currentWaypoint++;
        }
        if (currentWaypoint < path.vectorPath.Count)
        {
            return path.vectorPath[currentWaypoint];
        }
        return null;
    }
    public bool ReachedDestination(Vector2 targetPosition)
    {
        return Vector2.Distance(transform.position, targetPosition) < waypointDistance;
    }
}
