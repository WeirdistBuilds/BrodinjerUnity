using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DashForwardMove : NavMesh_Enemy_Base
{
    public float DashSpeed;
    public float DashMinTime, DashMaxTime;
    public float Acceleration;
    private float currentTime, dashTime;
    
    public override IEnumerator Move()
    {
        if(AnimationBase != null)
            AnimationBase.StartAnimation();
        dashTime = Random.Range(DashMinTime, DashMaxTime);
        currentTime = 0;
        agent.speed = DashSpeed;
        if(agent.enabled)
            agent.destination += agent.transform.forward * Time.deltaTime * DashSpeed;
        while (currentTime < dashTime && canMove && moving)
        {
            currentTime += Time.deltaTime;
            if(agent.enabled)
                agent.destination += agent.transform.forward * Time.deltaTime * DashSpeed;
            yield return fixedUpdate;
        }
        agent.speed = Speed;
        while (canMove && moving)
        {
            if(agent.enabled)
                agent.destination += agent.transform.forward * Time.deltaTime * DashSpeed;
            yield return fixedUpdate;
        }
        yield return fixedUpdate;
    }

    /*public override Enemy_Movement GetClone()
    {
        DashForwardMove temp = CreateInstance<DashForwardMove>();
        temp.Speed = Speed;
        temp.AngularSpeed = AngularSpeed;
        temp.Acceleration = Acceleration;
        temp.animation = animation;
        temp.DashMinTime = DashMinTime;
        temp.DashMaxTime = DashMaxTime;
        temp.DashSpeed = DashSpeed;
        return temp;
    }*/
}
