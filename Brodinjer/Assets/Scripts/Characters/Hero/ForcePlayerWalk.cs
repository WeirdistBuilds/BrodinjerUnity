using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ForcePlayerWalk : MonoBehaviour
{
    private Vector3 _moveVec;
    private readonly WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();

    public Animator anim;
    private ResetTriggers resetAnims;
    private CharacterController _cc;
    public float ForwardSpeed;
    private bool moving;
    private Coroutine moveFunc;
    public float AnimSpeedMax;

    public Transform Target;

    public UnityEvent ReachDestEvent;

    public bool AwakeOnStart;

    public string StartTrigger, EndTrigger, SpeedFloat, DirectionFloat;

    public Animation_Base animbase;
    
    private void Start()
    {
        moving = false;
        _cc = GetComponent<CharacterController>();
        resetAnims = anim.GetComponent<ResetTriggers>();
        _moveVec = Vector3.zero;
        if(AwakeOnStart)
            StartMove();
    }

    public void StartMove()
    {
        if (!moving)
        {
            moving = true;
            moveFunc = StartCoroutine(Move());
        }
    }

    public IEnumerator Move()
    {
        animbase.StopAnimation();
        _moveVec = Target.position - transform.position;
        _moveVec.y = 0;
        if(resetAnims)
            resetAnims.ResetAllTriggers();
        anim.SetTrigger(StartTrigger);
        Vector3 moveCheck = _moveVec;
        while (moveCheck.magnitude > .1f && moving)
        {

            _moveVec = _moveVec.normalized * ForwardSpeed;
            anim.SetFloat(SpeedFloat, getSpeed());
            anim.SetFloat(DirectionFloat, getMoveAngle());
            _cc.Move(_moveVec * Time.deltaTime);
            _moveVec = Target.position - transform.position;
            _moveVec.y = -30 * Time.deltaTime;
            moveCheck = _moveVec;
            moveCheck.y = 0;
            yield return fixedUpdate;
        }
        ReachDestEvent.Invoke();
        anim.SetFloat(SpeedFloat, 0);
        moving = false;
    }

    public void StopMove(bool movePlayer)
    {
        if(moveFunc!= null)
            StopCoroutine(moveFunc);
        if(resetAnims)
            resetAnims.ResetAllTriggers();
        if(EndTrigger != "")
            anim.SetTrigger(EndTrigger);
        moving = false;
        if (movePlayer)
        {
            _cc.transform.position = Target.position;
        }
    }

    public float getMoveAngle()
    {        
        return GetDirection(_cc.transform.InverseTransformDirection(_cc.velocity));
    }
    
    public float GetDirection(Vector3 moveDirection)
    {
        Vector3 collisionposition = moveDirection;
        collisionposition.y = 0;
        Vector3 transformposition = Vector3.zero;
        transformposition.y = 0;
        Vector3 target = collisionposition - transformposition;
        float angle = Vector3.Angle(target, Vector3.forward);
        Vector3 crossProduct = Vector3.Cross(target, Vector3.forward);
        if (crossProduct.y < 0)
        {
            angle = -angle;
        }

        angle /= 360;
        angle += .5f;
        return angle;
    }

    public float getSpeed()
    {
        Vector3 Velocity = _cc.velocity;
        Velocity.y = 0;
        return GeneralFunctions.ConvertRange(0, AnimSpeedMax, 0, 1, Velocity.magnitude);
    }
}
