using UnityEngine;

public class Circuit : MonoBehaviour
{
    public Transform[] wayPoints;

    public int LastWaypointIndex { get { return wayPoints.Length - 1; } }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < wayPoints.Length; i++)
        {
            int k = (i + 1) % wayPoints.Length;

            Vector3 prev = wayPoints[k].position;
            Vector3 next = wayPoints[i].position;

            Gizmos.DrawLine(prev, next);
        }
    }

    public int GetNextWayPointIndex(int currentWayPointIndex)
    {
        return currentWayPointIndex >= wayPoints.Length - 1 ? 0 : ++currentWayPointIndex;
    }
}
