using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy_Dash_Transform : Transform_Movement_Base
{
    public float DashSpeed;
    public float DashMinTime, DashMaxTime;
    public float Acceleration;
    private float currentTime, dashTime;
    private Vector3 newDestination;
    private float currentSpeed;
    public bool x, y, z;
    public SoundController AttackSound;
    
    public override IEnumerator Move()
    {
        if(AnimationBase != null)
            AnimationBase.StartAnimation();
        dashTime = Random.Range(DashMinTime, DashMaxTime);
        currentTime = 0;
        currentSpeed = Speed;
        AttackSound.Play();
        while (currentTime < dashTime && canMove && moving)
        {
            if (currentSpeed < DashSpeed)
            {
                currentSpeed += Time.deltaTime *Acceleration;
            }
            currentTime += Time.deltaTime;
            newDestination = (enemy.transform.forward * Time.deltaTime * currentSpeed) + enemy.transform.position;
            if (!x)
            {
                newDestination.x = enemy.transform.position.x;
            }

            if (!y)
            {
                newDestination.y = enemy.transform.position.y;
            }

            if (!z)
            {
                newDestination.z = enemy.transform.position.z;
            }
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, newDestination, currentSpeed*Time.deltaTime);
            yield return fixedUpdate;
        }
        while (canMove && moving)
        {
            if (currentSpeed > Speed)
            {
                currentSpeed -= Time.deltaTime * Acceleration;
            }
            newDestination = (enemy.transform.forward * Time.deltaTime * currentSpeed) + enemy.transform.position;
            if (!x)
                newDestination.x = enemy.transform.position.x;
            if (!y)
                newDestination.y = enemy.transform.position.y;
            if (!z)
                newDestination.z = enemy.transform.position.z;            
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, newDestination, currentSpeed*Time.deltaTime);
            yield return fixedUpdate;
        }
        yield return fixedUpdate;
    }
}
