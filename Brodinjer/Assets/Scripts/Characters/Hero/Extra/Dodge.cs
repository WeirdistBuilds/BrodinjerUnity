using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character/Player/Extra/Dodge")]
public class Dodge : CharacterControlExtraBase
{
    public TransformData targetObj;
    public string DodgeButton = "Jump";
    public float dodgeTime, dodgeRecoverTime, dodgeIncrease;
    private bool dodging;
    private float currentTime;

    private WaitForSeconds dodgerecover;
    
    private readonly WaitForFixedUpdate update = new WaitForFixedUpdate();

    public override void Init(Transform character, CharacterController _cc)
    {
        base.Init(character, _cc);
        dodgerecover = new WaitForSeconds(dodgeRecoverTime);
    }

    public override IEnumerator Move()
    {
        dodging = false;
        while (canMove)
        {
            while (targetObj != null)
            {
                if (Input.GetButtonDown(DodgeButton) && !dodging)
                {
                    dodging = true;
                    translate.extraControlled = true;
                    while (currentTime < dodgeTime)
                    {
                        //translate.Invoke(dodgeIncrease * Input.GetAxisRaw("Vertical"),
                            //dodgeIncrease * Input.GetAxisRaw("Horizontal"), false);
                        currentTime -= Time.deltaTime;
                        yield return update;
                    }
                    yield return dodgerecover;
                    translate.extraControlled = false;
                    dodging = false;
                }
                yield return update;
            }

            yield return update;
        }
    }
}
