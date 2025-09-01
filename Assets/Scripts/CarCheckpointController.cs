using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarCheckpointController : MonoBehaviour
{
    public int currentCheckpoint;
    public Text currentPosition;
    public List<CarCheckpointController> cars;
    public CheckpointController cpController;
    public bool playerCar;
    int position;
    int lap;
    Text lapText;

    void Start()
    {
        cars = new List<CarCheckpointController>(FindObjectsOfType<CarCheckpointController>());
        cpController = FindObjectOfType<CheckpointController>();
        lapText = FindObjectOfType<GameManager>().lapText;
        lapText.text = "Lap: 1";
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Checkpoint>())
        {
            Checkpoint cp = col.GetComponent<Checkpoint>();

            if (cp.index > currentCheckpoint)
            {
                if (!playerCar)
                    currentCheckpoint = cp.index;
                else if (currentCheckpoint + 1 == cp.index)
                    currentCheckpoint = cp.index;
            }
            else if (currentCheckpoint == cpController.checkpoints.Count && cp.index == 1)
            {
                lap++;
                if (lap >= FindObjectOfType<GameManager>().laps)
                {
                    if (playerCar)
                    {
                        FindObjectOfType<GameManager>().EndRace();
                        enabled = false;
                    }
                    else
                    {
                        GetComponent<WaypointFollower>().enabled = false;
                    }
                    GetComponent<HoverCarControl>().thrust = 0;
                }
                if (playerCar)
                    lapText.text = "Lap: " + (lap + 1).ToString();
                currentCheckpoint = 2;
            }
        }
    }


    void FixedUpdate()
    {
        float distanceToNextCheckpoint = float.MaxValue;
        if (playerCar)
        {
            position = 1;
            foreach (CarCheckpointController car in cars)
            {
                if (lap < car.lap)
                {
                    position++;
                    continue;
                }
                else if (lap > car.lap)
                {
                    continue;
                }
                if (car.currentCheckpoint > currentCheckpoint)
                {
                    position++;
                }
                else if (car.currentCheckpoint == currentCheckpoint && currentCheckpoint < cpController.checkpoints.Count - 1)
                {
                    distanceToNextCheckpoint = Vector3.Distance(transform.position, cpController.checkpoints[currentCheckpoint + 1].transform.position);
                    if (distanceToNextCheckpoint > Vector3.Distance(car.transform.position, cpController.checkpoints[currentCheckpoint + 1].transform.position))
                    {
                        position++;
                    }
                }
            }
            currentPosition.text = position.ToString();
        }
    }
}
