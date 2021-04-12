using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger_Enter_Event : Trigger_Event_Base
{
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(CheckTrigger(other));
    }

    public void SwapEvent(Unity_Event_Holder Event)
    {
        base.Event = Event.Event;
    }

    public void ForceEvent()
    {
        Event.Invoke();
    }
}
