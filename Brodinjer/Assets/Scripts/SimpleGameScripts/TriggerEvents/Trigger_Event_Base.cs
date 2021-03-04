using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class Trigger_Event_Base : MonoBehaviour
{
    public UnityEvent Event;
    public enum Check { Layer, Tag, Name, }
    public Check checksFor;
    public float waitTime, resetTime;

    public string objName;
    public LayerMask layer;
    public string tagName;

    public bool active = true;
    public bool isRunning;

    protected GameObject triggerCollider;

    private void Start()
    {
        isRunning = false;
    }

    public virtual IEnumerator CheckTrigger(Collider coll)
    {
        if (!isRunning)
        {
            isRunning = true;
            switch (checksFor)
            {
                case Check.Layer:
                    if (((1 << coll.gameObject.layer) & layer) != 0)
                    {
                        triggerCollider = coll.gameObject;
                        yield return new WaitForSeconds(waitTime);
                        RunEvent();
                        yield return new WaitForSeconds(resetTime);
                        isRunning = false;

                    }
                    break;
                case Check.Name:
                    if (coll.gameObject.name.Contains(objName))
                    {
                        triggerCollider = coll.gameObject;
                        yield return new WaitForSeconds(waitTime);
                        RunEvent();
                        yield return new WaitForSeconds(resetTime);
                        isRunning = false;

                    }
                    break;
                case Check.Tag:
                    if (coll.gameObject.CompareTag(tagName))
                    {
                        triggerCollider = coll.gameObject;
                        yield return new WaitForSeconds(waitTime);
                        RunEvent();
                        yield return new WaitForSeconds(resetTime);
                        isRunning = false;

                    }
                    break;
            }
            isRunning = false;

        }
    }

    public virtual void RunEvent()
    {
        if(active)
            Event.Invoke();
    }
    
    public int ToLayer (int bitmask ) {
        int result = bitmask>0 ? 0 : 31;
        while( bitmask>1 ) {
            bitmask = bitmask>>1;
            result++;
        }
        return result;
    }

    public void Active(bool value)
    {
        active = value;
    }
    
}
