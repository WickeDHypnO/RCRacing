using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour {

    HoverCarControl carControl;
    public List<GameObject> wheels;
    public List<Vector3> defaultPositions = new List<Vector3> { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero } ;
    public float suspensionHeight;

    void Start()
    {
        carControl = GetComponent<HoverCarControl>();
        for (int i = 0; i < wheels.Count; i++)
        {
            defaultPositions[i] = wheels[i].transform.localPosition;
        }
    }
	// Update is called once per frame
	void Update () {
		for(int i = 0; i<wheels.Count; i++)
        {
            wheels[i].transform.localPosition = new Vector3(defaultPositions[i].x, defaultPositions[i].y - (carControl.distances[i] * suspensionHeight), defaultPositions[i].z);
        }
	}
}
