using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Character/Common/Death/EventDeath")]
public class Death_Event : Death_Base
{
    [HideInInspector]
    public UnityEvent _event = new UnityEvent();
    public float waitTime;
    
    public override IEnumerator Death()
    {
        yield return new WaitForSeconds(waitTime);
        try
        {
            _event.Invoke();
        }
        catch
        {

        }
    }

    public override Death_Base GetClone()
    {
        Death_Event temp = CreateInstance<Death_Event>();
        temp.waitTime = waitTime;
        temp._event = _event;
        return temp;
    }

}
