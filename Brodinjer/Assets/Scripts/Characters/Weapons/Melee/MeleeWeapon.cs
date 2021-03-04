using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class MeleeWeapon : WeaponBase
{
    public GameObject knockbackObj, artObj;
    private WaitUntil waitforbutton;
    public float attackActiveTime01, attackActiveTime02, attackActiveTime03, attackcoolDownTime;
    private float currentTime;
    public UnityEvent AxUsedEvent;
    private WaitForFixedUpdate fixedUpdate;
    public float comboTime01, comboTime02, combo03Cooldown;
    public string ComboNumInteger;
    public float minComboWaitTime01, minComboWaitTime02;
    public float attackActivateTime01, attackActivateTime02, attackActivateTime03;
    public PlayerMovement playermove;
    public bool freezeWhenAttack;
    public float origwalkfor, origwalkside, origrunfor, origrunside;
    public CharacterTranslate origMovement;
    public CharacterRotate freezeRotation;
    public CharacterRotate origRotation;
    private WeaponDamageAmount damage;
    public float DamageAttack1, DamageAttack2, DamageAttack3;
    private bool running = false;
    public ParticleSystem WeaponTrail;
    public RandomSoundController Swing;
    [HideInInspector]public bool largeAttack;
    public GameObject SoundObj;

    private void Start()
    {
        running = false;
    }

    public override void Initialize()
    {
        artObj.SetActive(true);
        damage = knockbackObj.GetComponent<WeaponDamageAmount>();
        knockbackObj.SetActive(false);
        waitforbutton = new WaitUntil(CheckInput);
        fixedUpdate = new WaitForFixedUpdate();
        currWeapon = true;
        if (!running)
        {
            running = true;
            weaponFunc = StartCoroutine(Attack());
        }
    }

    public override IEnumerator Attack()
    {
        while (currWeapon)
        {
            while (frozen)
            {
                yield return fixedUpdate;
            }
            largeAttack = false;
            yield return waitforbutton;
            if (freezeWhenAttack)
            {
                playermove.SwapMovement(freezeRotation, origMovement);
                origMovement.RunForwardSpeed = 0;
                origMovement.RunSideSpeed = 0;
                origMovement.ForwardSpeed = 0;
                origMovement.SideSpeed = 0;
            }

            if (!frozen)
            {
                if (currWeapon)
                {
                    anim.ResetTrigger(AttackEndTrigger);
                    anim.SetTrigger(AttackTrigger);
                    anim.SetInteger(ComboNumInteger, 1);
                }
                Swing.Play();
                SoundObj.SetActive(true);
                yield return  new WaitForSeconds(attackActivateTime01);
                damage.DamageAmount = DamageAttack1;
                knockbackObj.SetActive(true);
                WeaponTrail.Play();
                AxUsedEvent.Invoke();
                currentTime = 0;
                while (currentTime < comboTime01 && !frozen && currWeapon)
                {
                    if (currentTime > attackActiveTime03 && knockbackObj.activeSelf)
                    {
                        //knockbackObj.SetActive(false);
                    }
                    if (CheckInput() && currentTime >= minComboWaitTime01)
                    {
                        //knockbackObj.SetActive(false);
                        anim.SetInteger(ComboNumInteger, 2);
                        Swing.Play();
                        yield return  new WaitForSeconds(attackActivateTime02);
                        damage.DamageAmount = DamageAttack2;
                        //knockbackObj.SetActive(true);
                        currentTime = 0;
                        while (currentTime < comboTime02 && !frozen && currWeapon)
                        {
                            if (currentTime > attackActiveTime02 && knockbackObj.activeSelf)
                            {
                                //knockbackObj.SetActive(false);
                            }
                            if (CheckInput()&& currentTime >= minComboWaitTime02)
                            {
                                //knockbackObj.SetActive(false);
                                anim.SetInteger(ComboNumInteger, 3);
                                Swing.Play();
                                largeAttack = true;
                                yield return  new WaitForSeconds(attackActivateTime03);
                                damage.DamageAmount = DamageAttack3;
                                //knockbackObj.SetActive(true);
                                currentTime = 0;
                                while (currentTime < attackActiveTime03 && !frozen && currWeapon)
                                {
                                    currentTime += Time.deltaTime;
                                    yield return fixedUpdate;
                                }
                                //knockbackObj.SetActive(false);
                                yield return new WaitForSeconds(combo03Cooldown);
                                currentTime = comboTime02 +comboTime01;
                            }

                            currentTime += Time.deltaTime;
                            yield return fixedUpdate;
                        }
                    }

                    currentTime += Time.deltaTime;
                    yield return fixedUpdate;
                }
                largeAttack = false;
                damage.DamageAmount = DamageAttack1;
                if (freezeWhenAttack)
                {
                    playermove.SwapMovement(origRotation, origMovement);
                    origMovement.RunForwardSpeed = origrunfor;
                    origMovement.RunSideSpeed = origrunside;
                    origMovement.ForwardSpeed = origwalkfor;
                    origMovement.SideSpeed = origwalkside;
                }
                SoundObj.SetActive(false);
                WeaponTrail.Stop();
                knockbackObj.SetActive(false);
                anim.SetInteger(ComboNumInteger, 0);
                anim.SetTrigger(AttackEndTrigger);
                yield return new WaitForSeconds(attackcoolDownTime);
            }

            yield return fixedUpdate;
        }
    }

    public override void End()
    {
        artObj.SetActive(false);
        knockbackObj.SetActive(false);
        currWeapon = false;
        anim.SetInteger(ComboNumInteger, 0);
        anim.SetTrigger(AttackEndTrigger);
        if(weaponFunc != null)
            StopCoroutine(weaponFunc);
        running = false;
        playermove.SwapMovement(origRotation, origMovement);
        origMovement.RunForwardSpeed = origrunfor;
        origMovement.RunSideSpeed = origrunside;
        origMovement.ForwardSpeed = origwalkfor;
        origMovement.SideSpeed = origwalkside;
        WeaponTrail.Stop();
    }
    
    private bool CheckInput()
    {
        if (Input.GetButtonDown(useButton))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Off()
    {
        if (weaponFunc != null)
            StopCoroutine(weaponFunc);
        running = false;
        origMovement.RunForwardSpeed = origrunfor;
        origMovement.RunSideSpeed = origrunside;
        origMovement.ForwardSpeed = origwalkfor;
        origMovement.SideSpeed = origwalkside;
        WeaponTrail.Stop();

    }

    public override void On()
    {
        Initialize();
        origMovement.RunForwardSpeed = origrunfor;
        origMovement.RunSideSpeed = origrunside;
        origMovement.ForwardSpeed = origwalkfor;
        origMovement.SideSpeed = origwalkside;
    }

    public override void Activate()
    {
        artObj.SetActive(true);
    }
}
