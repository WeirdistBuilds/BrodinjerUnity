using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Credits : MonoBehaviour
{
    public UnityEvent CreditsStart, CreditsEnd;
    private bool RunningCredits;

    private void Start()
    {
        RunningCredits = false;
    }
    private void Update()
    {
        if (RunningCredits)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopCredits();
            }
        }
    }

    public void StartCredits()
    {
        RunningCredits = true;
        CreditsStart.Invoke();
    }

    public void StopCredits()
    {
        RunningCredits = false;
        CreditsEnd.Invoke();
    }

    public void CreditsFinished()
    {
        RunningCredits =false;
    }
}
