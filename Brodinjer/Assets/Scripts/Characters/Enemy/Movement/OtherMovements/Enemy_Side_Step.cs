using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy_Side_Step : NavMesh_Enemy_Base
{
    public float sideAmount;
    public float DestinationOffset;
    private Vector3 right_dest, left_dest, _currDest;
    private bool _right = true;
    public float minTimeWait, maxTimeWait;
    public bool RotateToPlayer;
    
    public override IEnumerator Move()
    {
        right_dest = enemy.transform.position;
        right_dest += enemy.transform.right * sideAmount;
        left_dest = enemy.transform.position;
        left_dest += enemy.transform.right * -sideAmount;
        _currDest = right_dest;
        agent.destination = _currDest;
        _right = true;
        agent.updateRotation = false;
        agent.speed = Speed;
        while (moving)
        {
            if (CheckPosition(_currDest))
            {
                StartCoroutine(ChangeDest());
            }

            if (RotateToPlayer)
            {
                agent.gameObject.transform.LookAt(player);
            }
            yield return new WaitForSeconds(.1f);
        }
    }
    
    public virtual bool CheckPosition(Vector3 position)
    {
        if ((enemy.transform.position.x >= position.x - DestinationOffset
              && enemy.transform.position.x <= position.x + DestinationOffset)
            && (enemy.transform.position.z >= position.z - DestinationOffset
                 && enemy.transform.position.z <= position.z + DestinationOffset))
        {
            return true;
        }
        return false;
    }

    public virtual IEnumerator ChangeDest()
    {
        agent.speed = 0;
        if (_right)
        {
            _currDest = left_dest;
            _right = false;
        }
        else
        {
            _currDest = right_dest;
            _right = true;
        }
        yield return new WaitForSeconds(GetTime());
        agent.destination = _currDest;
        agent.speed = Speed;

    }

    public float GetTime()
    {
        return Random.Range(minTimeWait, maxTimeWait);
    }

    
    
}
