using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unity_Event_Holder : MonoBehaviour
{
    public UnityEvent Event;

    public void Call()
    {
        try
        {
            Event.Invoke();
        }
        catch
        {

        }
    }

}
