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
    public GameObject LeftSwipeAttackObjSide01, LeftSwipeAttackObjSide02;
    public GameObject RightSwipeAttackObjSide01, RightSwipeAttackObjSide02;
    private float minZFist, maxZFist, minXFist, maxXFist, maxZPlatform, minY, maxY, midpoint;
    private float x, y;
    private bool right;
    public bool Side01 = true;
    public BoolData side;
    public SoundController R_SwipeSound, L_SwipeSound;

    public void SwapSide()
    {
        Side01 = !side.value;
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
    }

    private void Awake()
    {
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
        yield return new WaitForSeconds(MovePauseTime);
        attackSound.Play();
        if (resetAnims)
            resetAnims.ResetAllTriggers();
        animator.SetTrigger(FistAttackTrigger);
        WeaponAttackobj.SetActive(true);
        SetPositionFist();
        yield return new WaitForSeconds(AttackActiveTime);
        WeaponAttackobj.SetActive(false);
        yield return new WaitForSeconds(CoolDownTime);

    }

    private IEnumerator SwipeAttack()
    {
        if (resetAnims)
            resetAnims.ResetAllTriggers();
        animator.SetTrigger(SwipeAttackTrigger);
        SetPositionSwipe(true);
        if(right)
            R_SwipeSound.Play();
        else
            L_SwipeSound.Play();
        yield return new WaitForSeconds(SwipeStartTime);
        if (right)
        {
            if (Side01)
            {
                RightSwipeAttackObjSide01.SetActive(true);
            }
            else
            {
                RightSwipeAttackObjSide02.SetActive(true);
            }
        }
        else
        {
            if (Side01)
            {
                LeftSwipeAttackObjSide01.SetActive(true);
            }
            else
            {
                LeftSwipeAttackObjSide02.SetActive(true);
            }
        }
        float currentTime = 0;
        while(currentTime < SwipeActiveTime)
        {
            SetPositionSwipe(false);
            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (right)
        {
            if (Side01)
            {
                RightSwipeAttackObjSide01.SetActive(false);
            }
            else
            {
                RightSwipeAttackObjSide02.SetActive(false);
            }
        }
        else
        {
            if (Side01)
            {
                LeftSwipeAttackObjSide01.SetActive(false);
            }
            else
            {
                LeftSwipeAttackObjSide02.SetActive(false);
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

    private void SetPositionSwipe(bool setside)
    {
        Vector3 position = player.position;
        if (Side01)
        {
            if (player.position.x > midpoint)
            {
                x = 1;
                if(setside)
                    right = true;
            }
            else
            {
                x = -1;
                if(setside)
                    right = false;
            }
        }
        else
        {
            if (player.position.x < midpoint)
            {
                x = 1;
                if(setside)
                    right = true;
            }
            else
            {
                x = -1;
                if(setside)
                    right = false;
            }
        }
        y = Mathf.Clamp(position.y, minY, maxY);
        y = GeneralFunctions.ConvertRange(minY, maxY, -1, 1, y);
        animator.SetFloat(XPositionName, x);
        animator.SetFloat(YPositionName, y);
    }

    public override void StopAttack()
    {
        base.StopAttack();
        RightSwipeAttackObjSide01.SetActive(false);
        RightSwipeAttackObjSide02.SetActive(false);
        LeftSwipeAttackObjSide01.SetActive(false);
        LeftSwipeAttackObjSide02.SetActive(false);
        WeaponAttackobj.SetActive(false);
    }
}
