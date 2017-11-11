using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfLevelTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider col)
    {
        if(col.tag == "VehicleCollider")
        {
            col.GetComponentInParent<HoverCarControl>().selfLeveling = false;
          //  col.GetComponentInParent<HoverCarControl>().lockZ = false;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "VehicleCollider")
        {
            col.GetComponentInParent<HoverCarControl>().selfLeveling = true;
           // col.GetComponentInParent<HoverCarControl>().lockZ = true;
        }
    }
}
