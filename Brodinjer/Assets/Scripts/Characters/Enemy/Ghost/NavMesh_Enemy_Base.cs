using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class NavMesh_Enemy_Base : Enemy_Movement
{
    [HideInInspector]
    public NavMeshAgent agent;

    //Basic Init
    protected override void Init()
    {
        canMove = true;
        agent = enemy.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = enemy.gameObject.AddComponent<NavMeshAgent>();
        }
        agent.speed = 0;
        agent.angularSpeed = AngularSpeed;
        if (AnimationBase != null)
        {
            Animation_Base temp = AnimationBase.GetClone();
            AnimationBase = temp;
            AnimationBase.Init(this, anim, player, agent);
        }
    }
    
    public override void StopMove()
    {
        moving = false;
        agent.speed = 0;
        agent.velocity = Vector3.zero;
        if (moveFunc != null)
        {
            StopCoroutine(moveFunc);
        }
        if(AnimationBase)
            AnimationBase.StopAnimation();
    }

}
