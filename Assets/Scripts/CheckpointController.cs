using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckpointController : MonoBehaviour {

    public List<Checkpoint> checkpoints;
    public DateTime lapTime = DateTime.MinValue;
    public DateTime raceTime = DateTime.MinValue;
    public bool startedTimer;

	[ContextMenu("Collect Checkpoints")]
    void CollectCheckpoints()
    {
        int iterator = 1;
        checkpoints.Clear();
        foreach(Checkpoint cp in GetComponentsInChildren<Checkpoint>())
        {
            cp.index = iterator;
            checkpoints.Add(cp);
            iterator++;
        }
    }

    void Update()
    {
        if (startedTimer)
        {
            raceTime.AddSeconds(Time.deltaTime);
            lapTime.AddSeconds(Time.deltaTime);
        }
    }
}
