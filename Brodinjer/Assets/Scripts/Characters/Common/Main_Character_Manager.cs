using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Character_Manager : Character_Manager
{
    public PlayerMovement movement;
    public WeaponManager weapons;
    private Vector3 difference;
    private CharacterController _cc;
    private bool running;
    private float currentTime;
    public Damage_Animation damageAnimation;
    public void SetDamageCooldown(float value)
    {
        damageCoolDown = value;
    }


    public override void Init()
    {
        base.Init();
        if(_cc == null)
            _cc = movement.GetComponent<CharacterController>();
        if(damageAnimation != null)
        {
            damageAnimation.Init(this, anim, player, null);
        }
        running = false;
        stunned = false;

    }

    public override IEnumerator Stun(float stuntime, float recoveryTime)
    {
        if (!stunned)
        {
            stunned = true;
            movement.Stun();
            weapons.WeaponFreeze();
            yield return new WaitForSeconds(stuntime);
            if (!dead)
            {
                anim.SetTrigger("Recover");
                anim.SetBool("RecoverBool", true);
            }
            yield return new WaitForSeconds(recoveryTime);
            anim.SetBool("RecoverBool", false);
            movement.UnStun();
            weapons.WeaponUnfreeze();
            stunned = false;

        }
        yield return new WaitForFixedUpdate();
    }

    public void TakeDamage(float amount)
    {
        if (canDamage && !dead)
        {
            Character.Health.TakeDamage(amount, false);
            if (damageSound)
                damageSound.Play();
            if (Character.Health.health.value <= 0)
            {
                return;
            }
        }
    }

    protected override void StartKnockback(WeaponDamageAmount other)
    {
        if (_cc != null)
        {
            if (other.knockbackDirection == Vector3.zero)
            {
                difference = _cc.transform.position - other.BaseObj.transform.position;
                difference.y = 0;
                difference = (difference.normalized * other.KnockbackForce);
            }
            else
            {
                difference = other.knockbackDirection * other.KnockbackForce;
            }
            if(!running)
                StartCoroutine(KnockCo(other));
        }

    }

    protected virtual IEnumerator KnockCo(WeaponDamageAmount other)
    {
        running = true;
        if (_cc != null)
        {
            currentTime = other.KnockbackTime;
            while (currentTime > 0)
            {
                _cc.Move(difference * Time.deltaTime);
                currentTime -= Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

        running = false;
    }

    public override void RunAnimation(string DamageAnimationTrigger, GameObject collisionObj)
    {
        if (DamageAnimationTrigger != "" && DamageAnimationTrigger != " ")
        {
            anim.SetTrigger(DamageAnimationTrigger);
        }
        else
        {
            if (damageAnimation != null)
                damageAnimation.StartAnimation(collisionObj.transform);
        }
    }

    private void OnDisable()
    {
        running = false;
    }
}
