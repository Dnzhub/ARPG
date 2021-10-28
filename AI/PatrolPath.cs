using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
{   
    const float wayPointGizmosRadius = 0.3f;
    private void OnDrawGizmos() 
    {
        for (int i = 0; i < transform.childCount; i++)
            {
                int j = NextIndex(i);
                Gizmos.DrawSphere(GetWayPoint(i), wayPointGizmosRadius);
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
        }
        }

        public int NextIndex(int i) // Bir sonraki waypointe hareket et
        {
            if(i + 1 == transform.childCount)
            {
                return 0; //Patrol path te her waypointin sonuna gelindiğinde başa dön.
            }
            return i + 1;
        }

        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}

