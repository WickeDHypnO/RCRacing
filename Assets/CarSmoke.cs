using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSmoke : MonoBehaviour
{
    public List<ParticleSystem> smoke;
    public float minSpeed;
    public float minAngle;
    HoverCarControl carControl;
    Rigidbody rigid;
    public AudioSource screechSound;

    void Start()
    {
        carControl = GetComponent<HoverCarControl>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (carControl.thrust > 0 && carControl.grounded && rigid.velocity.magnitude > minSpeed && Vector3.Angle(transform.forward, rigid.velocity) > minAngle)
        {
            if(!screechSound.isPlaying)
            screechSound.Play();
            foreach (var s in smoke)
            {
                s.Play();
            }
        }
        else
        {
            screechSound.Stop();
            foreach (var s in smoke)
            {
                s.Stop();
            }
        }
    }
}
