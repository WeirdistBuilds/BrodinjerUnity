using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character/Animation/Ribs")]
public class Rib_Animations : Animation_Base
{
    public string FrontAttack, BackAttack;
    public string directionFront, directionBack;
    private float angle, tempangle;
    public float minFrontAngle, maxFrontAngle;

    public override void StartAnimation()
    {
        if (anim != null)
        {
            angle = GetDirection();
            if (angle <= maxFrontAngle && angle >= minFrontAngle)
            {
                tempangle = ConvertRange(minFrontAngle, maxFrontAngle, 0, 1, angle);
                anim.SetFloat(directionFront, tempangle);
                anim.SetTrigger(FrontAttack);

            }
            else
            {
                if (angle > maxFrontAngle)
                {
                    tempangle = ConvertRange(maxFrontAngle, 1, 1, .5f, angle);

                }
                else
                {
                    tempangle = ConvertRange(0, minFrontAngle, .5f, 0, angle);

                }
                anim.SetFloat(directionBack, tempangle);
                anim.SetTrigger(BackAttack);

            }
        }
    }

    public override void StopAnimation()
    {
        //nothing
    }

    public virtual float GetDirection()
    {
        float angle =  GeneralFunctions.GetDirection(player.position, anim.transform.position);
        /*Vector3 collisionposition = player.position;
        collisionposition.y = 0;
        Vector3 transformposition = anim.transform.position;
        transformposition.y = 0;
        Vector3 target = collisionposition - transformposition;
        float directionalangle = Vector3.Angle(target, anim.transform.forward);
        Vector3 crossProduct = Vector3.Cross(target, anim.transform.forward);
        if (crossProduct.y < 0)
        {
            directionalangle = -directionalangle;
        }*/

        angle /= 360;
        angle += .5f;
        return angle;
    }
    
    public override Animation_Base GetClone()
    {
        Rib_Animations temp = CreateInstance<Rib_Animations>();
        temp.StartTriggerName = this.StartTriggerName;
        temp.StopTriggerName = this.StopTriggerName;
        temp.FrontAttack = FrontAttack;
        temp.BackAttack = BackAttack;
        temp.directionFront = directionFront;
        temp.directionBack = directionBack;
        temp.minFrontAngle = minFrontAngle;
        temp.maxFrontAngle = maxFrontAngle;
        return temp;
    }
    
}
