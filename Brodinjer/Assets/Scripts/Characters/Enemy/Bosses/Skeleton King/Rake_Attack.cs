using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rake_Attack : Enemy_Attack_Base
{
    public string AnimationTriggerName;
    public GameObject AttackObj2;
    
    public override IEnumerator Attack()
    {
        if(resetAnims)
            resetAnims.ResetAllTriggers();
        animator.SetTrigger(AnimationTriggerName);
        yield return new WaitForSeconds(AttackStartTime);
        WeaponAttackobj.SetActive(true);
        AttackObj2.SetActive(true);
        yield return new WaitForSeconds(AttackActiveTime);
        WeaponAttackobj.SetActive(false);
        AttackObj2.SetActive(false);
        yield return new WaitForSeconds(CoolDownTime);
    }
}
