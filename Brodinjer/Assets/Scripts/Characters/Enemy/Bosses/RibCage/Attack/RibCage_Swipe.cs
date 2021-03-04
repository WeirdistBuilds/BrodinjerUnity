using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RibCage_Swipe : Enemy_Attack_Base
{
    public float RotationSpeedAcceleration;
    public float MaxSpeed;
    public float RotationTime;
    public int numRotations;

    private float currentTime;
    private int currentRotation;
    private float currentVelocity;
    private bool Right;

    public List<GameObject> WeaponObjs;
    
    public override IEnumerator Attack()
    {
        attacking = true;
        if (animations)
        {
            animations.StartAnimation();
        }
        attackSound.Play();
        yield return new WaitForSeconds(AttackStartTime);
        foreach(var obj in WeaponObjs)
        {
            obj.SetActive(true);
        }
        /*currentRotation = 0;
        Right = false;
        while (currentRotation < numRotations)
        {
            currentTime = 0;
            currentVelocity = 0;
            while (currentVelocity < MaxSpeed)
            {
                currentTime += Time.deltaTime;
                currentVelocity += Time.deltaTime * RotationSpeedAcceleration;
                if (currentVelocity > MaxSpeed)
                {
                    currentVelocity = MaxSpeed;
                }

                if (Right)
                {
                    enemyObj.transform.Rotate(0,currentVelocity*Time.deltaTime, 0);
                }

                else
                {
                    enemyObj.transform.Rotate(0, -1*currentVelocity * Time.deltaTime, 0);
                }
                yield return new WaitForFixedUpdate();
            }

            currentTime = 0;
            while (currentTime < RotationTime)
            {
                currentTime += Time.deltaTime;
                if (Right)
                {
                    enemyObj.transform.Rotate(0,currentVelocity*Time.deltaTime, 0);
                }

                else
                {
                    enemyObj.transform.Rotate(0, -1*currentVelocity * Time.deltaTime, 0);
                }
                yield return new WaitForFixedUpdate();
            }

            while (currentVelocity > 0)
            {
                currentVelocity -= Time.deltaTime * RotationSpeedAcceleration;
                if (currentVelocity < 0)
                {
                    currentVelocity = 0;
                }

                if (Right)
                {
                    enemyObj.transform.Rotate(0,currentVelocity*Time.deltaTime, 0);
                }

                else
                {
                    enemyObj.transform.Rotate(0, -1*currentVelocity * Time.deltaTime, 0);
                }
                yield return new WaitForFixedUpdate();
            }
            
            currentRotation++;
            Right = !Right;
            yield return new WaitForFixedUpdate();
        }*/
        yield return new WaitForSeconds(AttackActiveTime);
        foreach (var obj in WeaponObjs)
        {
            obj.SetActive(false);
        }
        if (animations)
            animations.StopAnimation();
        yield return new WaitForSeconds(CoolDownTime);
        attacking = false;

    }
}
