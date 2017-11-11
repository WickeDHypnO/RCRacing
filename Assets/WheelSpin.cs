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
            if(Vector3.Angle(carControl.transform.forward, rigid.velocity) < 90)
            {
                transform.RotateAround(transform.right, rigid.velocity.magnitude * rotatingSpeed);
            }
            else
            {
                transform.RotateAround(transform.right, -rigid.velocity.magnitude * rotatingSpeed);
            }
        }
        else
        {
            transform.RotateAround(transform.right, carControl.thrust * rotatingSpeed);
        }
        if (canTurn)
        {
            if (carControl.turnValue != 0)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, turnAngle * carControl.turnValue, transform.localEulerAngles.z);
            }
            else
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
            }
        }
    }
}
