using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hand_Jump_Attack : Enemy_Attack_Base
{
    public float UpwardForce, ForwardForce;
    private Rigidbody enemyRigid;
    private Vector3 jumpdirection;
    public float InBetweenAttackTime;
    private NavMeshAgent agent;
    public float initTime;
    public override void Init()
    {
        base.Init();
        enemyRigid = enemyObj.GetComponent<Rigidbody>();
        if (!enemyRigid)
            enemyRigid = enemyObj.AddComponent<Rigidbody>();
    }

    public override void StartAttack()
    {
        if (!attacking)
        {
            attacking = true;
            StartCoroutine(Attack());
        }
    }

    public override IEnumerator Attack()
    {
        attacking = true;
        yield return new WaitForSeconds(initTime);
        if (animations)
        {
            animations.StartAnimation();
        }
        attackSound.Play();
        agent = enemyObj.GetComponent<NavMeshAgent>();
        agent.velocity = Vector3.zero;
        enemyRigid.freezeRotation = true;
        yield return new WaitForSeconds(AttackStartTime);
        if (attacking && canAttack)
        {
            agent.enabled = false;
            jumpdirection = (enemyRigid.transform.up * UpwardForce) + (enemyRigid.transform.forward * ForwardForce);
            enemyRigid.AddForce(jumpdirection, ForceMode.Impulse);
            if (WeaponAttackobj)
                WeaponAttackobj.SetActive(true);
            yield return new WaitForSeconds(AttackActiveTime);
            if (attacking && canAttack)
            {
                if (WeaponAttackobj)
                    WeaponAttackobj.SetActive(false);
                if (animations)
                    animations.StopAnimation();
                yield return new WaitForSeconds(CoolDownTime);
                agent.enabled = true;
                yield return new WaitForSeconds(InBetweenAttackTime);
            }
        }

        agent.enabled = true;
        attacking = false;
    }

}
