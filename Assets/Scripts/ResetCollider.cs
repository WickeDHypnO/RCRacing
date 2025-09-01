using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCollider : MonoBehaviour {

    int objects = 0;

	void OnTriggerEnter()
    {
        objects++;
    }

    void OnTriggerExit()
    {
        objects--;
        if(objects == 0)
        {
            GetComponentInParent<AIResetter>().SetAsCar();
        }
    }

    void OnEnable()
    {
        objects = 0;
        StartCoroutine(SetOpaqueIfNoObjects());
    }

    IEnumerator SetOpaqueIfNoObjects()
    {
        bool opaque = false;
        while(!opaque)
        {
            yield return new WaitForSeconds(0.5f);
            if(objects <= 0)
            {
                opaque = true;
                StopAllCoroutines();
                GetComponentInParent<AIResetter>().SetAsCar();
            }
        }
    }
}
