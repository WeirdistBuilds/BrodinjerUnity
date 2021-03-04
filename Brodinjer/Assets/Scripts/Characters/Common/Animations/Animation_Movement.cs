using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character/Animation/Movement")]
public class Animation_Movement : Animation_Base
{

    public string DirectionName;
    public string SpeedName;
    private bool animating;
    private Coroutine animateFunc;
    public float speedDif;
    private ResetTriggers resettrigger;
    //public float maxSpeed;
    
    public override void StartAnimation()
    {
        animating = true;
        anim.SetTrigger(StartTriggerName);
        animateFunc = caller.StartCoroutine(UpdateValues());
    }

    private IEnumerator UpdateValues()
    {
        resettrigger = anim.gameObject.GetComponent<ResetTriggers>();
        if(resettrigger != null)
            resettrigger.ResetAllTriggers();
        anim.SetTrigger(StartTriggerName);
        while (animating)
        {
            anim.SetFloat(SpeedName, agent.velocity.magnitude);
            anim.SetFloat(DirectionName, GetDirection());
            anim.speed = agent.velocity.magnitude * speedDif;
            if (anim.speed < 1)
            {
                anim.speed = 1;
            }
            yield return new WaitForSeconds(.5f);
        }
    }

    public override void StopAnimation()
    {
        anim.ResetTrigger(StartTriggerName);
        animating = false;
        if(animateFunc!= null)
            caller.StopCoroutine(animateFunc);
        if (StopTriggerName != "")
        {
            anim.SetTrigger(StopTriggerName);
        }
    }

    public virtual float GetDirection()
    {
        float angle =  GeneralFunctions.GetDirection(agent.velocity + anim.transform.position, anim.transform.position);
        angle /= 360;
        angle += .5f;
        return angle;
    }
    
    public override Animation_Base GetClone()
    {
        Animation_Movement temp = CreateInstance<Animation_Movement>();
        temp.StartTriggerName = this.StartTriggerName;
        temp.StopTriggerName = this.StopTriggerName;
        temp.DirectionName = DirectionName;
        temp.SpeedName = SpeedName;
        temp.speedDif = speedDif;
        //temp.maxSpeed = maxSpeed;
        return temp;
    }
    
}
