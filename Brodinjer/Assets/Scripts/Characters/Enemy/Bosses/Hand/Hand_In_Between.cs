using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hand_In_Between : Enemy_Follow_Base
{
    public float distanceFromFollowObj;
    public bool lookAtFollow;
    private Quaternion facingDirection;
    private Vector3 followDest, lookDirection;
    public bool FollowObjMain;
    private Vector3 target;
    public float offset;
    public Transform Destination01;
    public SoundController walkSound;
    public float MinFootstep, MaxFootstep;

    public override IEnumerator Move()
    {
        StartCoroutine(WalkSound());
        agent.speed = Speed;
        agent.updatePosition = true;
        float currentTime = 0;
        while (moving)
        {
            agent.updateRotation = true;
            if (lookAtFollow && moving)
            {
                target = player.transform.position;
                target = (target - agent.transform.position).normalized;
                facingDirection = Quaternion.LookRotation(target);
                Quaternion YRotation = Quaternion.Euler(agent.transform.rotation.eulerAngles.x,
                    facingDirection.eulerAngles.y, agent.transform.rotation.eulerAngles.z);
                if (!GeneralFunctions.CheckDestination(agent.transform.rotation.eulerAngles,
                    YRotation.eulerAngles, offset))
                {
                    agent.transform.rotation =
                        Quaternion.Lerp(agent.transform.rotation, YRotation, AngularSpeed * Time.deltaTime);
                }
            }

            if (FollowObjMain)
            {
                followDest = player.transform.position;
                followDest += Destination01.transform.forward * -distanceFromFollowObj;
            }
            else
            {
                followDest = Destination01.transform.position;
                followDest += player.transform.forward * -distanceFromFollowObj;
            }

            if (agent.enabled && currentTime > .5f)
            {
                currentTime = 0;
                agent.destination = followDest;
            }
            currentTime += .1f;
            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator WalkSound()
    {
        while (moving)
        {
            if (agent.velocity.magnitude >= .1f)
            {
                walkSound.Play();
                yield return new WaitForSeconds(GeneralFunctions.ConvertRange(0, Speed,
                    MaxFootstep, MinFootstep, agent.velocity.magnitude));
            }
            yield return new WaitForFixedUpdate();
        }
    }

}
