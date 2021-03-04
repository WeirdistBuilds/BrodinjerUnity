using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Character/Animation/Hero Damage Animation")]
public class Damage_Animation : Animation_Base
{

    public string DirectionName;

    public override Animation_Base GetClone()
    {
        Damage_Animation temp = CreateInstance<Damage_Animation>();
        temp.StartTriggerName = StartTriggerName;
        temp.StopTriggerName = StopTriggerName;
        temp.DirectionName = DirectionName;
        return temp;
    }

    public override void StartAnimation()
    {

    }

    public void StartAnimation(Transform hitDirection)
    {
        if(reset)
            reset.ResetAllTriggers();
        anim.SetFloat(DirectionName, GetDirection(hitDirection));

        anim.SetTrigger(StartTriggerName);
    }

    public override void StopAnimation()
    {
    }

    public virtual float GetDirection(Transform hitDirection)
    {
        Vector3 collisionposition = hitDirection.position;
        collisionposition.y = 0;
        Vector3 transformposition = player.position;
        transformposition.y = 0;
        Vector3 target = collisionposition - transformposition;
        float angle = Vector3.Angle(target, player.forward);
        Vector3 crossProduct = Vector3.Cross(target, player.forward);
        if (crossProduct.y < 0)
        {
            angle = -angle;
        }

        angle /= 360;
        angle += .5f;
        return angle;
    }
}
