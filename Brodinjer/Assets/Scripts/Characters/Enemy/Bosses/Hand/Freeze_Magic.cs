using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Freeze_Magic : Trigger_Event_Base
{
    public float freezeTime;
    private PlayerMovement playermove;
    private WeaponManager weapons;
    private Coroutine resetFunc;
    public UnityEvent OnTrigger, OnReset;
    public LayerMask ignoreLayer;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.gameObject);
        if(!isRunning)
            StartCoroutine(CheckTrigger(other));
    }

    public override void RunEvent()
    {
        playermove = triggerCollider.GetComponent<PlayerMovement>();
        weapons = triggerCollider.GetComponent<WeaponManager>(); 
        if (playermove)
        {
            playermove.Stun();
            if (weapons)
            {
                weapons.WeaponFreeze();
            }
            base.RunEvent();
            resetFunc = StartCoroutine(ResetMove());
        }

    }

    private IEnumerator ResetMove()
    {
        yield return new WaitForSeconds(freezeTime);
        if (playermove)
        {
            playermove.StartAll();
        }

        if (weapons)
        {
            weapons.WeaponUnfreeze();
        }

        isRunning = false;

    }

    public void Unfreeze()
    {
        if(resetFunc!= null)
            StopCoroutine(resetFunc);
        if (playermove)
        {
            playermove.UnStun();
        }

        if (weapons)
        {
            weapons.WeaponUnfreeze();
        }

        isRunning = false;
    }
}
