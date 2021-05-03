using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraRotationManager : MonoBehaviour
{
    public CameraRotationBase cameraRotation;
    public string CameraHorizontal, CameraVertical;
    public BoolData Paused;
    public bool rotateOnStart = true;
    public GameObject DeathCam, DrownCam, RotateObj;
    public Animator anim;
    public Z_Targeting targetscript;
    public float MinFloatRotate, CenterFloatRotate, MaxFloatRotate, AnimationOffset, 
        ShakeStartTime, ShakeEndTime;
    private Coroutine rotateFunc, swapFunc;
    private float currentRotate, mouseX, mouseY, currentTime;
    private Vector3 RotationAmount, targetposition;
    private bool drowned, targeting, dead, paused, inpause, moving, canRotate, shaking;

    
    private void Start()
    {
        dead = false;
        StartRotation();
        inpause = false;
        Cursor.lockState = CursorLockMode.Locked;
        if(rotateOnStart)
            SetRotate(true);
        AnimationOffset = 0;
        targeting = false;
        shaking = false;
    }

    public void PauseTime(bool val)
    {
        paused = val;
    }

    public void Die()
    {
        dead = true;
        if (!drowned)
        {
            DeathCam.SetActive(true);
            cameraRotation.cameraObject.SetActive(false);
        }
    }

    public void Drown()
    {
        drowned = true;
        DrownCam.SetActive(true);
        cameraRotation.cameraObject.SetActive(false);
        canRotate = false;
        DrownCam.transform.parent.GetComponent<Follow_Object>().TurnOffY();
    }

    public void ResetDrown()
    {
        if (!dead)
        {
            drowned = false;
            cameraRotation.cameraObject.SetActive(true);
            DrownCam.SetActive(false);
            canRotate = true;
            DrownCam.transform.parent.GetComponent<Follow_Object>().TurnOnY();
        }
    }

    public void SetRotate(bool val)
    {
        canRotate = val;
    }

    private void FixedUpdate()
    {
        if (Paused.value && !inpause)
        {
            inpause = true;
            StopRotation();
            Cursor.lockState = CursorLockMode.None;
        }
        else if (!Paused.value && inpause)
        {
            inpause = false;
            StartRotation();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void StartRotation()
    {
        moving = true;
        rotateFunc = StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        mouseX = 180;
        while (moving)
        {
            if (canRotate && !dead)
            {
                if (targetscript == null || !targetscript.isTargeting)
                {
                    if (cameraRotation.targeting)
                    {
                        UnTarget();
                    }
                    mouseX += (Input.GetAxis(CameraHorizontal) * cameraRotation.mouseXMultiplier *
                               cameraRotation.rotationSpeed) * Time.deltaTime;
                    mouseY -= (Input.GetAxis(CameraVertical) * cameraRotation.rotationSpeed *
                               cameraRotation.mouseYMultiplier) * Time.fixedDeltaTime;
                    mouseY = Mathf.Clamp(mouseY, cameraRotation.minCamAngle, cameraRotation.maxCamAngle);
                    transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);

                    if (mouseY <= 0)
                    {
                        anim.SetFloat("Aim Direction", GeneralFunctions.ConvertRange(cameraRotation.minCamAngle, 0, 0, .5f, mouseY) + AnimationOffset);
                    }
                    else
                    {
                        anim.SetFloat("Aim Direction",
                            GeneralFunctions.ConvertRange(0, cameraRotation.maxCamAngle, .5f, 1,
                                mouseY) + AnimationOffset);
                    }

                }
                else
                {
                    if (!cameraRotation.targeting)
                    {
                        Target(targetscript.objClosest);
                    }
                    if (targetscript.objClosest != null)
                    {
                        targetposition = targetscript.objClosest.transform.position;
                        targetposition += (Camera.main.transform.right * cameraRotation.targetOffset.x) + (Camera.main.transform.up * cameraRotation.targetOffset.y);
                        transform.LookAt(targetposition);
                    }

                    float angle = transform.rotation.eulerAngles.x;
                    if (angle > 180)
                    {
                        angle -= 360;
                    }
                    if (angle <= 0)
                    {
                        anim.SetFloat("Aim Direction", GeneralFunctions.ConvertRange(cameraRotation.minCamAngle, 0, 0, .5f, angle));
                    }
                    else
                    {
                        anim.SetFloat("Aim Direction", GeneralFunctions.ConvertRange(0, cameraRotation.maxCamAngle, .5f, 1, angle));
                    }

                    Vector3 rotate = transform.rotation.eulerAngles;
                    mouseY = rotate.x;
                    mouseX = rotate.y;
                }


            }


            yield return new WaitForFixedUpdate();
        }
    }
    
    public void SwapRotation(CameraRotationBase newBase)
    {
        if(cameraRotation.cameraObject != null)
            cameraRotation.cameraObject.SetActive(false);
        if(newBase.cameraObject != null)
            newBase.cameraObject.SetActive(true);
        cameraRotation = newBase;
        if(cameraRotation.shaking && !shaking)
        {
            StopShake();
        }
        else if(!cameraRotation.shaking && shaking)
        {
            StartShake();
        }
    }

    public void StopRotation()
    {
        moving = false;
        if(rotateFunc!= null)
            StopCoroutine(rotateFunc);
    }

    #region TIMESWAP

    public void StartTimeSwap(float time, CameraRotationBase origCamera, CameraRotationBase newCamera)
    {
        if (swapFunc == null)
        {
            currentTime = time;
            SwapRotation(newCamera);
            swapFunc = StartCoroutine(TimedSwap(origCamera));
        }
        else
        {
            currentTime = time;
        }
        
    }

    private IEnumerator TimedSwap(CameraRotationBase origCamera)
    {
        while (currentTime > 0 && !paused)
        {
            currentTime -= .1f;
            yield return new WaitForSeconds(.1f);
        }
        StopTimeSwap(origCamera);
    }

    public void StopTimeSwap(CameraRotationBase origCamera)
    {
        paused = false;
        if (swapFunc != null)
        {
            SwapRotation(origCamera);
            StopCoroutine(swapFunc);
        }
        else if(origCamera != cameraRotation)
        {
            SwapRotation(origCamera);
        }
        swapFunc = null;

    }

    #endregion

    #region TARGET

    public void Target(GameObject obj)
    {
        cameraRotation.Target(obj);
    }

    public void UnTarget()
    {
        cameraRotation.Untarget();
    }

    #endregion

    #region SHAKE
    public void StartShake()
    {
        if (!shaking)
        {
            shaking = true;
            StartCoroutine(ShakeChange(0, cameraRotation.ShakeMaxAmplitude, 0, cameraRotation.ShakeMaxFrequency, ShakeStartTime));
        }
    }

    private IEnumerator ShakeChange(float startamp, float endamp, float startfreq, float endfreq, float changetime)
    {
        float curtime = 0;
        while(curtime < changetime)
        {
            curtime += Time.deltaTime;
            cameraRotation.SetShake(Mathf.Lerp(startamp, endamp, GeneralFunctions.ConvertRange(0, changetime, 0, 1, curtime)),
                Mathf.Lerp(startfreq, endfreq, GeneralFunctions.ConvertRange(0, changetime, 0, 1, curtime)));
            yield return new WaitForFixedUpdate();
        }
    }

    public void StopShake()
    {
        if (shaking)
        {
            shaking = false;
            StartCoroutine(ShakeChange(cameraRotation.ShakeMaxAmplitude, 0, cameraRotation.ShakeMaxFrequency, 0, ShakeEndTime));
        }
    }
    #endregion

}
