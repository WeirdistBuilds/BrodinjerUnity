using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public CharacterRotate rotate, zRotation, deadRotate, stunnedRotate, thirdPersonRotate;
    public CharacterTranslate translate, deadTranslate, stunnedTranslate, thirdPersonTranslate;
    public List<CharacterControlExtraBase> extraControls = new List<CharacterControlExtraBase>();
    public Transform CharacterScalar, DirectionReference;
    public Animator anim;
    public bool MoveOnStart;
    public BoolData paused;
    public string IdleTrigger = "Idle";
    public Z_Targeting targetScript;

    private CharacterRotate prevRotate;
    private CharacterTranslate prevTranslate;
    private Coroutine rotateFunc, moveFunc, runFunc;
    private CharacterController _cc;
    private bool moving, rotating, extrarunning, active, stunned, dead, targeting, pauseInits;
    private ResetTriggers triggerReset;
    public RandomSoundController Jumpsound, Footsteps;
    public float MinFootstepInBetween, MaxFootstepInBetween;
    private bool walking;
    private float currentSpeed;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        triggerReset = anim.GetComponent<ResetTriggers>();
        dead = false;
        active = true;
        moving = false;
        rotating = false;
        extrarunning = false;
        pauseInits = false;
        Init();
        StartCoroutine(WalkSound());
    }

    public void Die()
    {
        Deactivate();
        translate = deadTranslate;
        translate.Init(this, _cc, DirectionReference, targetScript, anim, Jumpsound);
        rotate = deadRotate;
        rotate.Init(transform, DirectionReference);
        translate.canMove = true;
        translate.canRun = true;
        if (!moving)
        {
            moving = true;
            moveFunc = StartCoroutine(translate.Move());
            runFunc = StartCoroutine(translate.Run());
        }
        rotate.canRotate = true;
        if (!rotating)
        {
            rotating = true;
            rotateFunc = StartCoroutine(rotate.Rotate());
        }
    }

    private void FixedUpdate()
    {
        if (paused.value && !pauseInits)
        {
            pauseInits = true;
            StopAll();
        }
        else if (!paused.value && pauseInits)
        {
            pauseInits = false;
            StartAll();
        }
        if(targetScript!= null && targetScript.isTargeting && !targeting)
        {
            Target();
        }
        else if(targetScript != null && !targetScript.isTargeting && targeting)
        {
            UnTarget();
        }
        if (!dead && !stunned)
        {
            currentSpeed = _cc.velocity.magnitude;
            if (currentSpeed > 0.5f)
            {
                walking = true;
            }
            else
            {
                walking = false;
            }
        }
    }

    public void Stun()
    {
        if (!stunned && !dead)
        {
            SwapMovement(stunnedRotate, stunnedTranslate);
            stunned = true;          
        }
    }

    public void Drown()
    {
        stunned = true;
        StopAll();
        anim.SetTrigger("Drown");
    }

    public void UnStun()
    {
        if (stunned && !dead)
        {
            stunned = false;
            SwapMovement(thirdPersonRotate, thirdPersonTranslate);
            thirdPersonTranslate.SetRun(8);
            thirdPersonTranslate.SetWalk(2);
        }
    }

    private void Init()
    {
        rotate.Init(transform, DirectionReference);
        translate.Init(this, _cc, DirectionReference, targetScript, anim, Jumpsound);
        if (extraControls != null)
        {
            foreach (var extra in extraControls)
            {
                extra.Init(CharacterScalar, _cc);
            }
        }

        if (MoveOnStart)
        {
            MoveOnStart = false;
            StartAll();
        }
    }

    public void Deactivate()
    {
        active = false;
        StopAll();
    }

    public void EnterConv()
    {
        if (triggerReset)
            triggerReset.ResetAllTriggers();
        anim.SetTrigger(IdleTrigger);
        Deactivate();
    }

    public void ExitConv()
    {
        Activate();
        StartAll();
    }

    public void Activate()
    {
        active = true;
    }

    public void SwapMovement(CharacterRotate newRot, CharacterTranslate newTrans, List<CharacterControlExtraBase> extras = null)
    {
        if (!dead)
        {
            if (!targeting)
            {
                StopAll();
                rotate = newRot;
                translate = newTrans;
                extraControls = extras;
                Init();
                StartAll();
            }
            else
            {
                prevRotate = rotate;
                prevTranslate = translate;
            }
        }
    }

    public void StopAll()
    {
        StopMove();
        StopRotate();
        StopExtras();
    }

    public void StartAll()
    {
        if (active && !stunned)
        {
            StartMove();
            StartRotate();
            StartExtras();
        }
    }

    public void StopMove()
    {
        translate.canMove = false;
        translate.canRun = false;
        if (moveFunc != null)
            StopCoroutine(moveFunc);
        if (runFunc != null)
            StopCoroutine(runFunc);
        moving = false;
    }

    public void StartMove()
    {
        if (active && !stunned)
        {
            translate.canMove = true;
            translate.canRun = true;
            if (!moving)
            {
                moving = true;
                moveFunc = StartCoroutine(translate.Move());
                runFunc = StartCoroutine(translate.Run());
            }
        }
    }

    public void StopRotate()
    {
        rotate.canRotate = false;
        if (rotateFunc != null)
            StopCoroutine(rotateFunc);
        rotating = false;
    }

    public void StartRotate()
    {
        if (active && !stunned)
        {
            rotate.canRotate = true;
            if (!rotating)
            {
                rotating = true;
                rotateFunc = StartCoroutine(rotate.Rotate());
            }
        }
    }

    public void StartExtras()
    {
        if (active)
        {
            if (extraControls != null)
            {
                foreach (CharacterControlExtraBase extra in extraControls)
                {
                    extra.canMove = true;
                    if (!extrarunning)
                    {
                        extrarunning = true;
                        StartCoroutine(extra.Move());
                    }
                }
            }
        }
    }

    public void StopExtras()
    {
        if (extraControls != null)
        {
            foreach (CharacterControlExtraBase extra in extraControls)
            {
                extra.canMove = false;
            }
        }

        extrarunning = false;
    }

    public void SetTranslate(CharacterTranslate translate)
    {
        if (!targeting)
        {
            StopMove();
            this.translate = translate;
            if (_cc == null)
                _cc = GetComponent<CharacterController>();
            this.translate.Init(this, _cc, DirectionReference, targetScript, anim, Jumpsound);
            StartMove();
        }
        else
        {
            prevTranslate = translate;
        }

    }

    public void SetRotation(CharacterRotate rotate)
    {
        if (!targeting)
        {
            StopRotate();
            this.rotate = rotate;
            if (_cc == null)
                _cc = GetComponent<CharacterController>();
            this.rotate.Init(transform, DirectionReference);
            StartRotate();
        }
        else
        {
            prevRotate = rotate;
        }
    }

    private void Target()
    {
        prevRotate = rotate;
        prevTranslate = translate;
        SwapMovement(zRotation, translate, extraControls);
        targeting = true;
    }

    private void UnTarget()
    {
        targeting = false;
        SwapMovement(prevRotate, prevTranslate, extraControls);
    }

    private IEnumerator WalkSound()
    {
        while (!dead)
        {
            if (translate != null)
            {
                if (walking && _cc.isGrounded)
                {
                    Footsteps.Play();
                    yield return new WaitForSeconds(GeneralFunctions.ConvertRange(0, translate.RunForwardSpeed, MaxFootstepInBetween, MinFootstepInBetween, currentSpeed));
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

}
