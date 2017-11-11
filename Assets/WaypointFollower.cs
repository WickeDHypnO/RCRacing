using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    List<Transform> points;
    int currentPointIndex;
    HoverCarControl carControl;
    Vector3 currentPoint;
    Vector3 flattenedPosition;
    public float nextPointDistance;
    Rigidbody rigid;
    public float thrustMultiplier;
    public AnimationCurve turnValue;
    public float nextPointDistanceRandom;
    float defaultPointDistance;
    void Start()
    {
        points = FindObjectOfType<Waypoints>().points;
        carControl = GetComponent<HoverCarControl>();
        rigid = GetComponent<Rigidbody>();
        currentPoint = new Vector3(points[currentPointIndex].position.x, 0, points[currentPointIndex].position.z);
        defaultPointDistance = nextPointDistance;
    }


    void Update()
    {
        flattenedPosition = new Vector3(transform.position.x, 0, transform.position.z);
        if (Vector3.Distance(currentPoint, flattenedPosition) < nextPointDistance)
        {
            currentPointIndex++;
            if (currentPointIndex == points.Count)
                currentPointIndex = 0;
            currentPoint = new Vector3(points[currentPointIndex].position.x, 0, points[currentPointIndex].position.z);
            nextPointDistance = defaultPointDistance + Random.Range(-nextPointDistanceRandom, nextPointDistanceRandom);
        }
        Vector3 RelativeWaypointPosition = transform.InverseTransformPoint(new Vector3(
                                                   currentPoint.x,
                                                   0,
                                                   currentPoint.z));


        // by dividing the horizontal position by the magnitude, we get a decimal percentage of the turn angle that we can use to drive the wheels
        carControl.turnValue = turnValue.Evaluate(RelativeWaypointPosition.x / RelativeWaypointPosition.magnitude);

        // now we do the same for torque, but make sure that it doesn't apply any engine torque when going around a sharp turn...
        if (Mathf.Abs(carControl.turnValue) < 0.5)
        {
            carControl.thrust = RelativeWaypointPosition.z / RelativeWaypointPosition.magnitude * thrustMultiplier;
        }
        else
        {
            //if (rigid.velocity.magnitude > 1)
            //{
            carControl.thrust = 0.95f * thrustMultiplier;
            //}
        }
    }

    public void ResetToLastCheckpoint()
    {
        if (currentPointIndex > 0)
        {
            transform.position = points[currentPointIndex - 1].position;
        }
        else
        {
            transform.position = points[points.Count-1].position;
        }
        transform.eulerAngles = Vector3.zero;
        transform.forward = points[currentPointIndex].position - transform.position;
    }
}
