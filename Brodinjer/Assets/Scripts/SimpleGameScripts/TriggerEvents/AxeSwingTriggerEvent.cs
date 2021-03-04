using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSwingTriggerEvent : Trigger_Event_Base
{
    public RandomSoundController MeleeRandom;
    public SoundController StrongMelee;
    public MeleeWeapon melee;
    private GameObject obj;

    private void OnEnable()
    {
        isRunning = false;
    }
    private void OnDisable()
    {
        isRunning = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        obj = other.gameObject;
        StartCoroutine(CheckTrigger(other));
    }

    public override void RunEvent()
    {
        if (melee.largeAttack)
            StrongMelee.Play();
        else
            MeleeRandom.Play();
        base.RunEvent();
    }
}
