using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpin : MonoBehaviour {

    Rigidbody rigid;
    Rigidbody wheelRigid;
    HoverCarControl carControl;
    public float rotatingSpeed = 1;
    public bool canTurn;
    public float turnAngle;
    private float currentRotationAdd;
    private float angle;
    void Start()
    {
        wheelRigid = GetComponent<Rigidbody>();
        carControl = GetComponentInParent<HoverCarControl>();
        rigid = GetComponentInParent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (carControl.grounded)
        {
            // Calculate rotation based on the car's velocity
            float rotationAmount = carControl.DebugVelocity * rotatingSpeed;
        
            // Determine if we're going forward or backward
            angle = Vector3.Angle(carControl.transform.forward, rigid.linearVelocity);
        
            // Apply the proper rotation direction based on whether we're going forward or backward
            if (angle < 90)
            {
                // Going forward (relatively), add positive rotation
                transform.Rotate(Vector3.right * rotationAmount, Space.Self);
            }
            else
            {
                // Going backward (relatively), add negative rotation
                transform.Rotate(Vector3.right * -rotationAmount, Space.Self);
            }
        }
        else
        {
            transform.localEulerAngles += Vector3.right * (carControl.thrust * rotatingSpeed);
        }
        if (canTurn)
        {
            if (carControl.turnValue != 0)
            {
                transform.parent.localEulerAngles = new Vector3(0, turnAngle * carControl.turnValue, 0);
            }
            else
            {
                transform.parent.localEulerAngles = Vector3.zero;
            }
        }
    }
}
