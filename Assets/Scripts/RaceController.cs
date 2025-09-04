using System;
using System.Collections;
using Fusion;
using TMPro;
using UnityEngine;

public class RaceController : SimulationBehaviour
{
    public TextMeshProUGUI countdownText;
    private int count = 5;
    private void Start()
    {
        //FindFirstObjectByType<HoverCarControl>().enabled = false;
        countdownText.text = "Waiting for players...";
        StartCoroutine(Countdown());
    }

    public IEnumerator Countdown()
    {
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }
        countdownText.text = "";

        foreach (var car in FindObjectsByType<HoverCarControl>(FindObjectsSortMode.None))
        {
            car.enabled = true;
        }
    }
    
    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Countdown());
        }
    }
#endif
}
