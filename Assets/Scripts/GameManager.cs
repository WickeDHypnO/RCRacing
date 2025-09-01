using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool reverse;
    public Text countdownText;
    HoverCarControl playerCar;
    private bool startedCounting;
    float timer = 3f;
    public Text lapText;
    public int laps;

    void Awake()
    {
        if (reverse)
        {
            FindObjectOfType<Waypoints>().ReversePoints();
            foreach (HoverCarControl hcc in FindObjectsOfType<HoverCarControl>())
            {
                hcc.transform.Rotate(Vector3.up, 180);
            }
        }
    }

    void Start()
    {
        foreach (WaypointFollower wf in FindObjectsOfType<WaypointFollower>())
        {
            wf.enabled = false;
        }
        foreach (HoverCarControl hcc in FindObjectsOfType<HoverCarControl>())
        {
            if (hcc.playerControlled)
            {
                playerCar = hcc;
                hcc.playerControlled = false;
            }
        }
        startedCounting = true;
    }

    void Update()
    {
        if (startedCounting)
        {
            timer -= Time.deltaTime;
            if (timer > 0)
            {
                countdownText.text = ((int) timer + 1).ToString();
            }
            else if (timer <= 0 && timer > -1)
            {
                foreach (WaypointFollower wf in FindObjectsOfType<WaypointFollower>())
                {
                    wf.enabled = true;
                }
                playerCar.playerControlled = true;
                countdownText.text = "Go!";
            }
            else if (timer <= -1)
            {
                countdownText.gameObject.SetActive(false);
                startedCounting = false;
            }
        }

    }

    public void EndRace()
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = "Finish!";
        playerCar.playerControlled = false;
    }
}
