using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic_Slam_Attack : Enemy_Attack_Base
{
    public string xPositionName, zPositionName;
    public string animationTriggerName, AnimationAttackTriggerName;
    public Transform Corner01, Corner02;
    private float x, z, scalex, scalez;
    private float minX, maxX, minZ, maxZ;
    public Transform ScaleZone;
    private Transform scalezonetemp;
    public Vector3 MaxScale;
    public float ScaleIncreaseTime;
    public float ScaleActiveTime;
    public bool Side01 = true;
    public BoolData side;
    public SoundController MagicSound;
    public Transform WristPosition;

    public void SwapSide()
    {
        Side01 = !side.value;
        Setup();
    }

    private void Setup()
    {
        if (Corner01.position.x > Corner02.position.x)
        {
            maxX = Corner01.position.x;
            minX = Corner02.position.x;
        }
        else
        {
            maxX = Corner02.position.x;
            minX = Corner01.position.x;
        }

        if (Corner01.position.z > Corner02.position.z)
        {
            maxZ = Corner01.position.z;
            minZ = Corner02.position.z;
        }
        else
        {
            maxZ = Corner02.position.z;
            minZ = Corner01.position.z;
        }
    }

    private void Awake()
    {
        ScaleZone.gameObject.SetActive(false);
        ScaleZone.transform.localScale = Vector3.zero;
        Setup();
    }

    public override IEnumerator Attack()
    {
        if (resetAnims)
            resetAnims.ResetAllTriggers();
        animator.SetTrigger(animationTriggerName);
        yield return new WaitForSeconds(AttackStartTime);
        SetPosition();
        yield return new WaitForSeconds(MovePauseTime);
        attackSound.Play();
        if (resetAnims)
            resetAnims.ResetAllTriggers();
        animator.SetTrigger(AnimationAttackTriggerName);
        WeaponAttackobj.SetActive(true);
        yield return new WaitForSeconds(AttackActiveTime);
        StartCoroutine(IncreaseScaleObj());
        WeaponAttackobj.SetActive(false);
        yield return new WaitForSeconds(CoolDownTime);

    }

    public void SetPosition()
    {
        Vector3 position = player.position;
        scalex = Mathf.Clamp(position.x, minX, maxX);
        scalez = Mathf.Clamp(position.z, minZ, maxZ);
        if (Side01)
        {
            x = GeneralFunctions.ConvertRange(minX, maxX, -1, 1, scalex);
            z = GeneralFunctions.ConvertRange(minZ, maxZ, -1, 1, scalez);
        }
        else
        {
            x = GeneralFunctions.ConvertRange(minX, maxX, 1, -1, scalex);
            z = GeneralFunctions.ConvertRange(minZ, maxZ, 1, -1, scalez);
        }
        animator.SetFloat(xPositionName, x);
        animator.SetFloat(zPositionName, z);
    }

    private IEnumerator IncreaseScaleObj()
    {
        MagicSound.Play();
        scalezonetemp = Instantiate(ScaleZone);
        Vector3 pos = ScaleZone.position;
        pos.x = WristPosition.position.x;
        pos.z = WristPosition.position.z;
        scalezonetemp.position = pos;
        scalezonetemp.gameObject.SetActive(true);
        float currentTime = 0;
        while(currentTime < ScaleIncreaseTime)
        {
            currentTime += Time.deltaTime;
            scalezonetemp.transform.localScale = Vector3.Lerp(Vector3.zero, MaxScale, 
                GeneralFunctions.ConvertRange(0, ScaleIncreaseTime, 0, 1, currentTime));
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(ScaleActiveTime);
        Destroy(scalezonetemp.gameObject);
        MagicSound.Stop();
    }
}
