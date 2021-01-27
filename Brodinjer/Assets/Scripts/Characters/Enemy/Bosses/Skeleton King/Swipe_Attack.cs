using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe_Attack : Enemy_Attack_Base
{
    public float SwipeStartTime, SwipeCoolDownTime, SwipeActiveTime;
    public string FistInitAnimationTrigger, FistAttackTrigger, SwipeAttackTrigger;
    public string XPositionName, YPositionName;
    public Transform FistDest01, FistDest02;
    public Transform PlatformMin, Bottom, Top, MidPoint;
    public List<GameObject> LeftSwipeAttackObj;
    public List<GameObject> RightSwipeAttackObj;
    private float minZFist, maxZFist, minXFist, maxXFist, maxZPlatform, minY, maxY, midpoint;
    private float x, y;
    private bool right;
    public bool Side01 = true;
    private List<WeaponDamageAmount> LeftWeapons, RightWeapons;

    public void SetSide(bool val)
    {
        Side01 = val;
        Setup();
    }

    public void SwapSide()
    {
        Side01 = !Side01;
        Setup();
    }

    private void Setup()
    {
        minY = Bottom.position.y;
        maxY = Top.position.y;
        maxZPlatform = PlatformMin.position.z;
        midpoint = MidPoint.position.x;
        if (FistDest01.position.x > FistDest02.position.x)
        {
            maxXFist = FistDest01.position.x;
            minXFist = FistDest02.position.x;
        }
        else
        {
            maxXFist = FistDest02.position.x;
            minXFist = FistDest01.position.x;
        }

        if (FistDest01.position.z > FistDest02.position.z)
        {
            maxZFist = FistDest01.position.z;
            minZFist = FistDest02.position.z;
        }
        else
        {
            maxZFist = FistDest02.position.z;
            minZFist = FistDest01.position.z;
        }
        if (Side01)
        {
            foreach(var weap in LeftWeapons)
            {
                weap.SetKnockbackDirection(new Vector3(1,.5f,1));
            }
            foreach(var weap in RightWeapons)
            {
                weap.SetKnockbackDirection(new Vector3(-1, .5f, 1));
            }
        }
        else
        {
            foreach(var weap in LeftWeapons)
            {
                weap.SetKnockbackDirection(new Vector3(-1, .5f, -1));
            }
            foreach(var weap in RightWeapons)
            {
                weap.SetKnockbackDirection(new Vector3(1, .5f, -1));
            }
        }
    }

    private void Awake()
    {
        LeftWeapons = new List<WeaponDamageAmount>();
        foreach(var weap in LeftSwipeAttackObj)
        {
            LeftWeapons.Add(weap.GetComponent<WeaponDamageAmount>());
        }
        RightWeapons = new List<WeaponDamageAmount>();
        foreach (var weap in RightSwipeAttackObj)
        {
            RightWeapons.Add(weap.GetComponent<WeaponDamageAmount>());
        }
        Setup();

    }

    public override IEnumerator Attack()
    {
        if (Side01)
        {
            if (player.position.z > maxZPlatform)
            {
                yield return StartCoroutine(FistAttack());
            }
            else
            {
                yield return StartCoroutine(SwipeAttack());
            }
        }
        else
        {
            if (player.position.z < maxZPlatform)
            {
                yield return StartCoroutine(FistAttack());
            }
            else
            {
                yield return StartCoroutine(SwipeAttack());
            }
        }
    }

    private IEnumerator FistAttack()
    {
        if (resetAnims)
            resetAnims.ResetAllTriggers();
        animator.SetTrigger(FistInitAnimationTrigger);
        yield return new WaitForSeconds(AttackStartTime);
        SetPositionFist();
        yield return new WaitForSeconds(MovePauseTime);
        if (resetAnims)
            resetAnims.ResetAllTriggers();
        animator.SetTrigger(FistAttackTrigger);
        WeaponAttackobj.SetActive(true);
        yield return new WaitForSeconds(AttackActiveTime);
        WeaponAttackobj.SetActive(false);
        yield return new WaitForSeconds(CoolDownTime);

    }

    private IEnumerator SwipeAttack()
    {
        if (resetAnims)
            resetAnims.ResetAllTriggers();
        SetPositionSwipe();
        animator.SetTrigger(SwipeAttackTrigger);
        yield return new WaitForSeconds(SwipeStartTime);
        if (right)
        {
            foreach (var weapon in RightSwipeAttackObj)
            {
                weapon.SetActive(true);
            }
        }
        else
        {
            foreach (var weapon in LeftSwipeAttackObj)
            {
                weapon.SetActive(true);
            }
        }
        yield return new WaitForSeconds(SwipeActiveTime);
        if (right)
        {
            foreach (var weapon in RightSwipeAttackObj)
            {
                weapon.SetActive(false);
            }
        }
        else
        {
            foreach (var weapon in LeftSwipeAttackObj)
            {
                weapon.SetActive(false);
            }
        }
        yield return new WaitForSeconds(SwipeCoolDownTime);

    }

    private void SetPositionFist()
    {
        Vector3 position = player.position;
        x = Mathf.Clamp(position.x, minXFist, maxXFist);
        y = Mathf.Clamp(position.z, minZFist, maxZFist);
        if (Side01)
        {
            x = GeneralFunctions.ConvertRange(minXFist, maxXFist, -1, 1, x);
            y = GeneralFunctions.ConvertRange(minZFist, maxZFist, -1, 1, y);
        }
        else
        {
            x = GeneralFunctions.ConvertRange(minXFist, maxXFist, 1, -1, x);
            y = GeneralFunctions.ConvertRange(minZFist, maxZFist, 1, -1, y);
        }
        animator.SetFloat(XPositionName, x);
        animator.SetFloat(YPositionName, y);
    }

    private void SetPositionSwipe()
    {
        Vector3 position = player.position;
        if (Side01)
        {
            if (player.position.x > midpoint)
            {
                x = 1;
                right = true;
            }
            else
            {
                x = -1;
                right = false;
            }
        }
        else
        {
            if (player.position.x < midpoint)
            {
                x = 1;
                right = true;
            }
            else
            {
                x = -1;
                right = false;
            }
        }
        y = Mathf.Clamp(position.y, minY, maxY);
        y = GeneralFunctions.ConvertRange(minY, maxY, -1, 1, y);
        animator.SetFloat(XPositionName, x);
        animator.SetFloat(YPositionName, y);
    }
}
