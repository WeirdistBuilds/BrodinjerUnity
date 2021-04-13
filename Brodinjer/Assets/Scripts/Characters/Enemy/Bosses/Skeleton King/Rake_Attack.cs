using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rake_Attack : Enemy_Attack_Base
{
    public string AnimationTriggerName;
    public GameObject AttackObj2, PalmAttackObj;
    public SoundController swipeSound;
    public float waittime01, waittime02;
    public GameObject damageObject;
    
    public override IEnumerator Attack()
    {
        if(resetAnims)
            resetAnims.ResetAllTriggers();
        animator.SetTrigger(AnimationTriggerName);
        yield return new WaitForSeconds(AttackStartTime);
        PalmAttackObj.SetActive(true);
        yield return new WaitForSeconds(waittime01);
        attackSound.Play();
        yield return new WaitForSeconds(.2f);
        PalmAttackObj.SetActive(false);
        damageObject.SetActive(true);
        yield return new WaitForSeconds(waittime02);
        swipeSound.Play();
        yield return new WaitForSeconds(.25f);
        damageObject.SetActive(false);
        AttackObj2.SetActive(true);
        WeaponAttackobj.SetActive(true);
        yield return new WaitForSeconds(AttackActiveTime);
        WeaponAttackobj.SetActive(false);
        AttackObj2.SetActive(false);
        yield return new WaitForSeconds(CoolDownTime);
    }
}
