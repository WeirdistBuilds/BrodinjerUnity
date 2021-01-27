﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableObjectDirectional : ScalableObjectBase
{
    public Vector3 MinimumScaler, MaximumScaler;
    public float TimeFromMinToMax;
    private Vector3 minimumScale, maximumScale;
    private float currentScalePoint;
    public Transform ObjectToScale;
    public Scaling_VFX scalingVFX;
    private float currentfloat;
    public float autoDecreaseSpeed;
    public Animator elevatorAnim;

    private void Start()
    {
        minimumScale = ObjectToScale.localScale;
        minimumScale.x *= MinimumScaler.x;
        minimumScale.y *= MinimumScaler.y;
        minimumScale.z *= MinimumScaler.z;
        maximumScale = ObjectToScale.localScale;
        maximumScale.x *= MaximumScaler.x;
        maximumScale.y *= MaximumScaler.y;
        maximumScale.z *= MaximumScaler.z;
        currentScalePoint = GeneralFunctions.ConvertRange(minimumScale.y, maximumScale.y, 0, TimeFromMinToMax, ObjectToScale.localScale.y);
        if(elevatorAnim!= null)
            elevatorAnim.SetFloat("Pos", GeneralFunctions.ConvertRange(0, TimeFromMinToMax, 0, 1, currentScalePoint));
    }
    public override void ScaleDown(bool deltaTimed)
    {
        currentScalePoint = Mathf.Clamp(currentScalePoint - Time.deltaTime, 0, TimeFromMinToMax);
        currentfloat = GeneralFunctions.ConvertRange(0, TimeFromMinToMax, 0, 1, currentScalePoint);
        if(elevatorAnim != null)
            elevatorAnim.SetFloat("Pos", currentfloat);
        ObjectToScale.localScale = Vector3.Lerp(minimumScale, maximumScale, currentfloat);
        if (scalingVFX != null)
            scalingVFX.Scale(currentfloat);
    }

    public override void ScaleUp(bool deltaTimed)
    {
        currentScalePoint = Mathf.Clamp(currentScalePoint + Time.deltaTime, 0, TimeFromMinToMax);
        currentfloat = GeneralFunctions.ConvertRange(0, TimeFromMinToMax, 0, 1, currentScalePoint);
        ObjectToScale.localScale = Vector3.Lerp(minimumScale, maximumScale, currentfloat);
        if(elevatorAnim != null)
            elevatorAnim.SetFloat("Pos", currentfloat);
        if (scalingVFX != null)
            scalingVFX.Scale(currentfloat);
    }

    public override void SetInit(float scale)
    {
    }

    public override void SetMax()
    {
    }

    public override void AutoScale(float time, bool Up)
    {
        StartCoroutine(Auto(time, Up));
    }

    private IEnumerator Auto(float time, bool Up)
    {
        Debug.Log("Auto Scale: " + gameObject);
        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            if (Up)
            {
                currentScalePoint = Mathf.Clamp(currentScalePoint + (Time.deltaTime * autoDecreaseSpeed), 0, TimeFromMinToMax);

            }
            else
            {
                currentScalePoint = Mathf.Clamp(currentScalePoint - (Time.deltaTime * autoDecreaseSpeed), 0, TimeFromMinToMax);
            }
            currentfloat = GeneralFunctions.ConvertRange(0, TimeFromMinToMax, 0, 1, currentScalePoint);
            ObjectToScale.localScale = Vector3.Lerp(minimumScale, maximumScale, currentfloat);
            if(elevatorAnim != null)
                elevatorAnim.SetFloat("Pos", currentfloat);
            if (scalingVFX != null)
                scalingVFX.Scale(currentfloat);
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("End Auto Scale: " + gameObject);
    }
}
