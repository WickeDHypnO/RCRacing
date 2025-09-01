using System.Collections.Generic;
using UnityEngine;

public class HoverCarControl : MonoBehaviour
{
    Rigidbody body;
    float deadZone = 0.1f;
    public float groundedDrag = 3f;
    public float maxVelocity = 50;
    public float hoverForce = 1000;
    public float gravityForce = 1000f;
    public float hoverHeight = 1.5f;
    public GameObject[] hoverPoints;
    public List<float> distances = new List<float>() { 0, 0, 0, 0 };
    public float forwardAcceleration = 8000f;
    public float reverseAcceleration = 4000f;
    public float thrust = 0f;
    public AnimationCurve accelerationCurve;
    public bool grounded = false;
    public bool selfLeveling;
    public float turnStrength = 1000f;
    public float turnValue = 0f;
    public float DebugVelocity;
    public bool playerControlled = true;
    public ParticleSystem[] dustTrails = new ParticleSystem[2];
    int layerMask;
    public float selfLevelingStrength;
    public bool lockZ;
    bool[] groundedWheels = new bool[4];
    Vector3 refVelocity;
    public bool autoAccelerate = false;
    public float mobileTurnValue;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.centerOfMass = Vector3.down;

        layerMask = 1 << LayerMask.NameToLayer("Vehicle");
        layerMask = ~layerMask;
    }

    //void OnDrawGizmos()
    //{

    //    RaycastHit hit;
    //    for (int i = 0; i < hoverPoints.Length; i++)
    //    {
    //        var hoverPoint = hoverPoints[i];
    //        if (Physics.Raycast(hoverPoint.transform.position,
    //                            -Vector3.up, out hit,
    //                            hoverHeight,
    //                            layerMask))
    //        {
    //            Gizmos.color = Color.blue;
    //            Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
    //            Gizmos.DrawSphere(hit.point, 0.5f);
    //        }
    //        else
    //        {
    //            Gizmos.color = Color.red;
    //            Gizmos.DrawLine(hoverPoint.transform.position,
    //                           hoverPoint.transform.position - Vector3.up * hoverHeight);
    //        }
    //    }
    //}

    public void Turn(float value)
    {
        mobileTurnValue += value;
    }

    void Update()
    {
        float acceleration = Input.GetAxis("Vertical");
        float turnAxis = Input.GetAxis("Horizontal");
        if (autoAccelerate)
        {
            if (acceleration > -0.1f)
                acceleration = 1.0f;
            if (Mathf.Abs(turnAxis) < 0.1f)
                turnAxis = mobileTurnValue;
        }

        // Get thrust input
        if (playerControlled)
        {
            thrust = 0.0f;
            turnValue = 0.0f;
            if (acceleration > deadZone)
                thrust = acceleration * forwardAcceleration;
            else if (acceleration < -deadZone)
                thrust = acceleration * reverseAcceleration;
            if (Mathf.Abs(turnAxis) > deadZone)
                turnValue = turnAxis;
        }
    }

    void FixedUpdate()
    {
        //  Do hover/bounce force
        RaycastHit hit;
        grounded = false;
        for (int i = 0; i < groundedWheels.Length; i++)
        {
            groundedWheels[i] = false;
        }
        for (int i = 0; i < hoverPoints.Length; i++)
        {
            var hoverPoint = hoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit, hoverHeight, layerMask))
            {
                body.AddForceAtPosition(Vector3.up * hoverForce * (1.0f - (hit.distance / hoverHeight)), hoverPoint.transform.position);
                distances[i] = hit.distance;
                grounded = true;
                groundedWheels[i] = true;
            }
            else
            {
                distances[i] = hoverHeight;
            }
        }
        for (int i = 0; i < hoverPoints.Length; i++)
        {
            var hoverPoint = hoverPoints[i];
            if (!groundedWheels[i])
            {
                if (selfLeveling)
                {
                    // Self levelling - returns the vehicle to horizontal when not grounded and simulates gravity
                    if (transform.position.y > hoverPoint.transform.position.y)
                    {
                        body.AddForceAtPosition(hoverPoint.transform.up * gravityForce * selfLevelingStrength, hoverPoint.transform.position);
                        body.AddForceAtPosition(Vector3.up * -gravityForce, hoverPoint.transform.position);
                    }
                    else
                    {
                        body.AddForceAtPosition(hoverPoint.transform.up * -gravityForce * selfLevelingStrength, hoverPoint.transform.position);
                        body.AddForceAtPosition(Vector3.up * -gravityForce, hoverPoint.transform.position);
                    }
                }
                else
                {
                    // body.AddForceAtPosition(Vector3.up * -gravityForce, hoverPoint.transform.position);
                }
            }
        }

        var emissionRate = 0;
        if (grounded)
        {
            body.linearDamping = groundedDrag;
            emissionRate = 10;
        }
        else
        {
            body.linearDamping = 0.1f;
            thrust /= 100f;
            turnValue /= 100f;
        }
        DebugVelocity = body.linearVelocity.magnitude;
        if (Mathf.Abs(thrust) > 0 && grounded)
        {

            body.AddForce(transform.forward * thrust * accelerationCurve.Evaluate(body.linearVelocity.sqrMagnitude / (body.linearVelocity.normalized * maxVelocity).sqrMagnitude));
        }

        if (turnValue > 0)
        {
            if (thrust > 0)
            {
                body.AddRelativeTorque(Vector3.up * turnValue * turnStrength);
            }
            else if (thrust < 0)
            {
                body.AddRelativeTorque(Vector3.up * -turnValue * turnStrength);
            }
        }
        else if (turnValue < 0)
        {
            if (thrust > 0)
            {
                body.AddRelativeTorque(Vector3.up * turnValue * turnStrength);
            }
            else if (thrust < 0)
            {
                body.AddRelativeTorque(Vector3.up * -turnValue * turnStrength);
            }
        }

        // Limit max velocity
        if (body.linearVelocity.sqrMagnitude > (body.linearVelocity.normalized * maxVelocity).sqrMagnitude)
        {
            body.linearVelocity = body.linearVelocity.normalized * maxVelocity;
        }

        if (Vector3.Angle(transform.up, Vector3.down) < 10f)
        {
            body.linearDamping = 10f;
        }

        if (lockZ)
            transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0), ref refVelocity, 0.7f);
    }
}