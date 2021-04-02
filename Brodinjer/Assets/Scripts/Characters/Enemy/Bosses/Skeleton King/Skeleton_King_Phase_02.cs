using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_King_Phase_02 : Phase_Base
{
    /*
     *dragging his hand violently along the arena in one fell swipe
     * or just trying to directly slam his fist down on Malik
     * He will occasionally use magic by slamming a splayed, glowing hand to the ground.
     *      Select pieces of debris will begin to glow and scale down
     */

    public List<Enemy_Attack_Base> PhaseAttacks;
    public List<float> Percentages;
    private List<float> nums;
    private List<float> alteredNums;
    private List<float> percent;
    private Enemy_Attack_Base attack;
    public float minInBetween, maxInBetween;
    private float waitTime;
    public float DamageWaitTime;
    private bool damaged;
    public Animator anim;
    private ResetTriggers resetTriggers;
    public float PhaseStartDelayTime;
    private int currentAttack = -1;

    private void Start()
    {
        resetTriggers = anim.gameObject.GetComponent<ResetTriggers>();
        nums = new List<float>();
        float currentNum = 0;
        for (int i = 0; i < Percentages.Count - 1; i++)
        {
            currentNum += Percentages[i];
            nums.Add(currentNum);
        }

    }

    public List<float> Setup(List<float> num)
    {
        List<float> temp = new List<float>();
        float currentNum = 0;
        for (int i = 0; i < Percentages.Count - 1; i++)
        {
            currentNum += num[i];
            temp.Add(currentNum);
        }
        return temp;
    }
    public override void StartPhase()
    {
        if (!currentPhase)
        {
            alteredNums = nums;
            percent = Percentages;
            currentPhase = true;
            StartCoroutine(PhaseStart());
        }
    }
    private IEnumerator PhaseStart()
    {
        StartPhaseEvent.Invoke();
        yield return new WaitForSeconds(PhaseStartDelayTime);
        phaseFunc = StartCoroutine(RunPhase());
    }
    public override IEnumerator RunPhase()
    {
        while (currentPhase)
        {
            //Wait for Time
            waitTime = Random.Range(minInBetween, maxInBetween);
            yield return new WaitForSeconds(waitTime);
            //Randomize Attack
            attack = PhaseAttacks[RandomAttack()];
            attack.attacking = true;
            yield return StartCoroutine(attack.Attack());
            if (damaged)
                yield return new WaitForSeconds(DamageWaitTime);
            else
            {
                if (resetTriggers)
                    resetTriggers.ResetAllTriggers();
                anim.SetTrigger("Idle");
            }
            damaged = false;
        }
    }
    private int RandomAttack()
    {
        if (PhaseAttacks.Count > 1)
        {
            int randint = Random.Range(0, 100);
            float randFloat = randint / 100f;
            float currentnum = 0;
            for (int i = 0; i < nums.Count; i++)
            {
                if (randFloat >= currentnum && randFloat < alteredNums[i])
                {
                   /* if(i == currentAttack)
                    {
                        percent[i] -= .1f;
                        for(int j = 0; j < percent.Count; j++)
                        {
                            if( currentAttack != j)
                            {
                                percent[j] += (.1f / (percent.Count - 1));
                            }
                        }
                        alteredNums = Setup(percent);
                    }
                    else
                    {
                        currentAttack = i;
                        percent = Percentages;
                        alteredNums = nums;
                    }*/
                    return i;
                }
                currentnum = alteredNums[i];
            }
            /*if (percent.Count-1 == currentAttack)
            {
                percent[percent.Count - 1] -= .1f;
                for (int j = 0; j < percent.Count; j++)
                {
                    if (currentAttack != j)
                    {
                        percent[j] += (.1f / (percent.Count - 1));
                    }
                }
                alteredNums = Setup(percent);
            }
            else
            {
                currentAttack = percent.Count - 1;
                percent = Percentages;
                alteredNums = nums;
            }*/
            return nums.Count;

        }
        return 0;
    }
    public override void StopPhase()
    {
        anim.SetTrigger("Reset");
        if (phaseFunc != null)
            StopCoroutine(phaseFunc);
        currentPhase = false;
    }
    public override void StopAttack()
    {
        if (attack != null)
            attack.attacking = false;
    }
    public override void StopDamage()
    {
        damaged = true;
        if (attack != null)
            attack.attacking = false;
    }

    public override void ResumeAttack()
    {
        currentPhase = true;
        alteredNums = nums;
        percent = Percentages;
        phaseFunc = StartCoroutine(RunPhase());
    }
}
