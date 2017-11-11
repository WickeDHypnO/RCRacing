using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {

    public List<Transform> points = new List<Transform>();

    public void ReversePoints()
    {
            points.Reverse();
    }

	void OnDrawGizmos()
    {
        foreach(Transform t in points)
        {
            Gizmos.DrawSphere(t.position, 1f);
        }
        for(int i =0; i<points.Count; i++)
        {
            if(i==points.Count-1)
            {
                Gizmos.DrawLine(points[i].position, points[0].position);
            }
            else
            {
                Gizmos.DrawLine(points[i].position, points[i+1].position);
            }
        }
    }
}
