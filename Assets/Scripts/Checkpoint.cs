using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Checkpoint : MonoBehaviour {

    public bool isSplit;
    public bool isStart;

    public DateTime splitTime = DateTime.MinValue;
    public int index;

    public void SetSplit()
    {
        if(splitTime != DateTime.MinValue)
        {
            Debug.Log("Split: " + (splitTime - DateTime.MinValue).Minutes + ":" + (splitTime - DateTime.MinValue).Seconds);
        }
        splitTime = FindObjectOfType<CheckpointController>().lapTime;
    }

    public void StartNextLap()
    {
        FindObjectOfType<CheckpointController>().lapTime = DateTime.MinValue;
    }
}
