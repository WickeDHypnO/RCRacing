using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour {

    public AudioSource sound;
    public float maxPitch, minPitch;
    float pitchDiff;
    HoverCarControl carControl;
    Rigidbody carRigid;

    void Start()
    {
        carControl = GetComponent<HoverCarControl>();
        carRigid = GetComponent<Rigidbody>();
        pitchDiff = maxPitch - minPitch;
    }

    void FixedUpdate()
    {
        if(carControl.grounded)
        {
            sound.pitch = minPitch + (pitchDiff * (carRigid.velocity.magnitude / carControl.maxVelocity) * 2f);
        }
        else
        {
            if (Mathf.Abs(carControl.thrust) > 0)
            {
                sound.pitch = maxPitch;
            }
            else
            {
                sound.pitch = minPitch + (Mathf.Abs(carControl.thrust / 20000f) * pitchDiff);
            }      
        }
    }
}
