using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Character_Manager : MonoBehaviour
{
    public Character_Base Character;
    private Character_Base _characterTemp;
    public bool MainCharacter = false;
    
    public LayerMask DamageLayer;
    public float damageCoolDown;

    public bool StunTime;

    protected bool damaged, stunned;
    [HideInInspector]public bool dead;

    public bool ResetTriggerBool = true;
    public Animation_Base DamageAnimation;
    public Animator anim;
    public NavMeshAgent agent;
    protected Transform player;

    public bool Knockback;

    public bool damageAnimate = true;
    public bool canDamage = true;

    public SoundController damageSound;
    ResetTriggers resettrigger;

    protected virtual void Start()
    {
        damaged = false;
        stunned = false;
        dead = false;
        player = FindObjectOfType<PlayerMovement>().transform;
        if (DamageAnimation)
        {
            Animation_Base temp = DamageAnimation.GetClone();
            DamageAnimation = temp;
            DamageAnimation.Init(this, anim, player, agent);
        }
        resettrigger = anim.GetComponent<ResetTriggers>();
        Init();
    }

    public void SetDamageAnimate(bool val)
    {
        damageAnimate = val;
    }

    public void SetCanDamage(bool val)
    {
        canDamage = val;
    }

    public virtual void Init()
    {
        if (!MainCharacter)
        {
            _characterTemp = Character.getClone();
            Character = _characterTemp;
        }
        Character.Init(this, transform, MainCharacter);
        if (GetComponent<Death_Event_Setup>() != null && Character.Health.Death_Version is Death_Event)
        {
            Death_Event death = Character.Health.Death_Version as Death_Event;
            if (death != null)
            {
                death._event = GetComponent<Death_Event_Setup>().DeathEvent;
            }
        }
    }

    public virtual void TakeDamage(float amount, bool armor)
    {
        if (canDamage && !dead)
        {
            Character.Health.TakeDamage(amount, armor);
            if (damageSound && !MainCharacter)
                damageSound.Play();
            if (Character.Health.health.value <= 0)
            {
                return;
            }
            if (damageSound && MainCharacter)
                damageSound.Play();
            
        }
    }

    private IEnumerator OnTriggerEnter(Collider coll)
    {
        if (canDamage && !damaged && !dead)
        {
            //layermask == (layermask | (1 << layer))
            if (((1 << coll.gameObject.layer) & DamageLayer) != 0)
            {
                WeaponDamageAmount temp = coll.GetComponent<WeaponDamageAmount>();
                if (temp != null)
                {
                    if (StunTime)
                    {
                        StartCoroutine(Stun(temp.StunTime, temp.RecoveryTime));
                    }

                    damaged = true;
                    /*if (resettrigger)
                        resettrigger.ResetAllTriggers();*/
                    if (damageAnimate)
                    {
                        RunAnimation(temp.DamageAnimationTrigger, coll.gameObject);
                    }

                    if (!temp.SingleHit || (temp.SingleHit && !temp.hit))
                    {
                        temp.hit = true;
                        if(temp.singleHitTimer > 0)
                        {
                            StartCoroutine(temp.SingelHitTimer());
                        }
                        TakeDamage(temp.DamageAmount, temp.DecreasedbyArmor);
                    }

                    if (Knockback)
                    {
                        StartKnockback(temp);
                    }

                    yield return new WaitForSeconds(damageCoolDown);
                    damaged = false;
                }
            }
        }
    }

    protected abstract void StartKnockback(WeaponDamageAmount other);
    
    public int ToLayer (int bitmask ) {
        int result = bitmask>0 ? 0 : 31;
        while( bitmask>1 ) {
            bitmask = bitmask>>1;
            result++;
        }
        return result;
    }

    public abstract IEnumerator Stun(float stuntime, float recoveryTime);
    
    public virtual void RunAnimation(string DamageAnimationTrigger, GameObject collisionObj)
    {
        if (DamageAnimationTrigger != "" && DamageAnimationTrigger != " ")
        {
            Debug.Log("Run Animation");
            anim.SetTrigger(DamageAnimationTrigger);
        }
        else
        {
            Debug.Log("Animation Trigger");
            if (DamageAnimation != null)
                DamageAnimation.StartAnimation();
        }
    }
    
}
