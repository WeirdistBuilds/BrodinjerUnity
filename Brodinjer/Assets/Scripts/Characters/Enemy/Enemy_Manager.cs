using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class Enemy_Manager : MonoBehaviour
{
    public Enemy_Movement currentMove;
    public Enemy_Attack_Base currentAttack;

    public bool moveOnStart;

    private float currentPauseTime;
    public bool paused, dead, stunned;

    private void Start()
    {
        dead = false;
        stunned = false;
        if(moveOnStart)
            StartMove();
    }

    public void Stun()
    {
        stunned = true;
        StopMove();
        StopAttack();
    }

    public void UnStun()
    {
        stunned = false;
        StartMove();
    }

    public void StartMove()
    {
        if (!dead && !stunned)
        {
            currentMove.enabled = true;
            currentMove.StartMove();
        }
    }

    public void SwapMovement(Enemy_Movement newMovement)
    {
        currentMove.StopMove();
        currentMove.enabled = false;
        currentMove = newMovement;
    }

    public void StopMove()
    {
        currentMove.StopMove();
        currentMove.enabled = false;
    }

    public void StartAttack()
    {
        if (!dead&&!stunned)
        {
            Debug.Log("Attack");
            currentAttack.enabled = true;
            currentAttack.StartAttack();
            if (!currentAttack.attackWhileMoving)
            {
                if (!paused)
                {
                    paused = true;
                    StartCoroutine(PauseMove());
                }
            }
        }
    }

    private IEnumerator PauseMove()
    {
        StopMove();
        yield return new WaitForSeconds(.1f);
        while (currentAttack.attacking)
        {
            yield return new WaitForFixedUpdate();
        }
        StartMove();
        paused = false;
    }

    public void SwapAttack(Enemy_Attack_Base newAttack)
    {
        currentAttack.StopAttack();
        currentAttack.enabled = false;
        currentAttack = newAttack;
    }

    public void StopAttack()
    {
        currentAttack.StopAttack();
        currentAttack.enabled = false;
    }

    public void Die()
    {
        dead = true;
        StopAttack();
        StopMove();
    }
}
