using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Attack_Melee_Taunt : Enemy_Attack_Base
{
    public List<float> attackPercentages;
    public List<bool> tauntBools;
    private float randomNum;
    public string animationFloat;
    public List<float> attackActiveTimes;
    public List<float> attackStartTimes;
    public List<float> attackCoolDownTimes;
    public List<float> attackDamage;
    private WeaponDamageAmount damage;
    private int currentnum;
    public List<SoundController> attackSounds;
    public Enemy_Manager manager;


    public override void Init()
    {
        base.Init();
        damage = WeaponAttackobj.GetComponent<WeaponDamageAmount>();
    }

    public override IEnumerator Attack()
    {
        if (manager.stunned)
        {
            StopAllCoroutines();
            attacking = false;
            manager.StartMove();
        }
        attacking = true;
        currentnum = GetRandom();
        if (damage)
            damage.DamageAmount = attackDamage[currentnum];
        yield return new WaitForFixedUpdate();
        if (manager.stunned)
        {
            StopAllCoroutines();
            attacking = false;
            manager.StartMove();
        }
        if (resetAnims)
            resetAnims.ResetAllTriggers();
        animations.StartAnimation();
        yield return new WaitForSeconds(attackStartTimes[currentnum]);
        attackSounds[currentnum].Play();
        if (!tauntBools[currentnum])
        {
            WeaponAttackobj.SetActive(true);
        }
        yield return new WaitForSeconds(attackActiveTimes[currentnum]);
        WeaponAttackobj.SetActive(false);
        animations.StopAnimation();
        yield return new WaitForSeconds(attackCoolDownTimes[currentnum]);
        attacking = false;
        manager.StartMove();

    }

    private int GetRandom()
    {
        randomNum = Random.Range(0.0f, 1.0f);
        for (int i = 0; i < attackPercentages.Count-1; i++)
        {
            if (randomNum >= (attackPercentages[i]) && randomNum < attackPercentages[i + 1])
            {
                animator.SetFloat(animationFloat, (i/(attackPercentages.Count-1.0f)));
                return i;
            }
        }
        animator.SetFloat(animationFloat, 1);
        return attackPercentages.Count-1;

    }
    

}
