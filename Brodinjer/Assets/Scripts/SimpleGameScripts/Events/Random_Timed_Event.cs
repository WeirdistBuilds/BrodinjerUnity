using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Random_Timed_Event : MonoBehaviour
{
    public float minTime, maxTime;
    public UnityEvent Event;
    public bool OnAwake;
    public bool Repeat;
    public bool repeating;
    public bool RunOnStart = false;
    public float StartDelay;

    private Coroutine waitFunc;
    
    private IEnumerator Start()
    {
        if (OnAwake)
        {
            repeating = Repeat;
            yield return new WaitForSeconds(StartDelay);
            if (RunOnStart)
            {
                Event.Invoke();
                if (repeating)
                {
                    StartCoroutine(Wait());
                }
            }
            else
            {
                StartCoroutine(Wait());
            }
        }
        
    }

    public void Call()
    {
        repeating = Repeat;
        waitFunc = StartCoroutine(Wait());
    }


    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        Event.Invoke();
        while (repeating)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            Event.Invoke();
        }
    }

    public void Stop()
    {
        if (waitFunc != null)
        {
            StopCoroutine(waitFunc);
        }

        repeating = false;
    }
    

}
