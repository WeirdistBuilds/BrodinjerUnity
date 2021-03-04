using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StraightForwardMove : NavMesh_Enemy_Base
{
    public float Acceleration;
    
    public override IEnumerator Move()
    {
        agent.speed = Speed;
        agent.acceleration = Acceleration;
        if (AnimationBase!= null)
            AnimationBase.StartAnimation();
        while (moving)
        {
            if(agent.enabled)
                agent.destination += agent.transform.forward * Time.deltaTime * Speed;
            yield return fixedUpdate;
        }
    }

    /*public override Enemy_Movement GetClone()
    {
        StraightForwardMove temp = CreateInstance<StraightForwardMove>();
        temp.Speed = Speed;
        temp.AngularSpeed = AngularSpeed;
        temp.Acceleration = Acceleration;
        temp.animation = animation;
        return temp;
    }*/
}
