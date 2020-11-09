﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class Enemy_Manager : MonoBehaviour
{
    public Enemy_Movement Movement_Version;
    private Enemy_Movement _movementTemp;
    
    public List<Transform> Destinations;

    public GameObject WeaponObj;
    public Enemy_Attack_Base Attack;
    private Enemy_Attack_Base _attackTemp;

    public Transform Player;
    public Animator animator;

    private bool canAttack;
    private bool canMove;
    private bool paused;

    public bool AwakeOnStart = true;

    private bool stunned;

    private void Start()
    {
        paused = false;
        canAttack = true;
        canMove = true;
        Init();
        if(AwakeOnStart)
            StartMove();
    }

    public void deactivateMove()
    {
        canMove = false;
        Movement_Version.deactiveMove();
        StopMove();
    }

    public void deactivateAttack()
    {
        Attack.DeactivateAttack();
        canAttack = false;
        StopAttack();
    }

    public void activateMove()
    {
        canMove = true;
        Movement_Version.activateMove();
    }

    public void activateAttack()
    {
        Attack.ActivateAttack();
        canAttack = true;
    }

    public void Stun()
    {
        stunned = true;
    }

    public void UnStun()
    {
        stunned = false;
    }

    #region INIT FUNCTIONS

    public void Init()
    {
        InitMovement();
        InitAttack();
    }

    public void InitMovement()
    {
        _movementTemp = Movement_Version.GetClone();
        Movement_Version = _movementTemp;
        Movement_Version.Init(gameObject, this, Player, Destinations, animator);
    }

    public void InitAttack()
    {
        if (Attack != null)
        {
            _attackTemp = Attack.getClone();
            Attack = _attackTemp;
            Attack.Init(this, WeaponObj, Player, animator, gameObject);
        }
    }
    #endregion

    #region SETTER FUNCTIONS

    public void SetNewMovement(Enemy_Movement movement)
    {
        Movement_Version.StopMove();
        Movement_Version = movement;
        InitMovement();
    }

    public void SetNewAttack(Enemy_Attack_Base attack)
    {
        Attack.StopAttack();
        Attack = attack;
        InitAttack();
    }

    public void SetNewWeaponObject(GameObject obj)
    {
        Attack.StopAttack();
        WeaponObj = obj;
        InitAttack();
    }

    public void ClearDestinations()
    {
        Destinations.Clear();
    }
    
    public void AddNewDestination(Transform dest)
    {
        Destinations.Add(dest);
    }
    
    #endregion

    #region START FUNCTIONS

    public void StartMove()
    {
        if(canMove)
            Movement_Version.StartMove();
    }

    public void StartAttack()
    {
        if (canAttack)
        {
            if (Attack != null && !Attack.attackWhileMoving)
            {
                if (!paused)
                {
                    paused = true;
                    StartCoroutine(PauseMove());
                }
            }

            Attack.StartAttack();
        }
    }

    private IEnumerator PauseMove()
    {
        if (Attack != null)
        {
            Movement_Version.StopMove();
            yield return new WaitForSeconds(Attack.CoolDownTime + 
                                            Attack.AttackActiveTime + Attack.AttackStartTime);
            Movement_Version.StartMove();
        }

        paused = false;
    }

    #endregion

    #region STOP FUNCTIONS

    public void StopMove()
    {
        Movement_Version.StopMove();
    }

    public void StopAttack()
    {
        if (Attack != null)
        {
            if (!Attack.attackWhileMoving)
            {
                Movement_Version.StartMove();
            }

            Attack.StopAttack();
        }
    }

    #endregion
    
}
