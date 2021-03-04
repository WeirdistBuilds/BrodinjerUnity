using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transform_Flee_Route : Transform_Movement_Base
{
    public List<Transform> destinations;
    private int currentDestIndex;
    private Transform currentDestination;
    private Vector3 backDest, forwardDest;
    private Vector3 targetRotation, targetDestination;
    private Quaternion facingDirection;
    public float RotationOffset, TranslationOffset;
    public bool x, y, z;
    public float ySpeed;
    public float minChangeDestTime, maxChangeDestTime;
    public float DistanceFromPlayer;
    private bool forward;
    public override IEnumerator Move()
    {
        forward = true;
        if (AnimationBase != null)
            AnimationBase.StartAnimation();
        currentDestination = destinations[0];
        currentDestIndex = 0;
        while (moving)
        {
            targetRotation = currentDestination.transform.position;
            targetRotation = (targetRotation - enemy.transform.position).normalized;
            facingDirection = Quaternion.LookRotation(targetRotation);
            Quaternion YRotation = Quaternion.Euler(enemy.transform.rotation.eulerAngles.x,
                facingDirection.eulerAngles.y, enemy.transform.rotation.eulerAngles.z);
            if (!GeneralFunctions.CheckDestination(enemy.transform.rotation.eulerAngles,
                YRotation.eulerAngles, RotationOffset))
            {
                enemy.transform.rotation =
                    Quaternion.Lerp(enemy.transform.rotation, YRotation, AngularSpeed * Time.deltaTime);
            }

            targetDestination = enemy.transform.position + (enemy.transform.forward * Speed * Time.deltaTime);
            targetDestination.y = enemy.transform.position.y;
            if (!x)
            {
                targetDestination.x = enemy.transform.position.x;
            }

            if (!z)
            {
                targetDestination.z = enemy.transform.position.z;
            }

            enemy.transform.position =
                Vector3.MoveTowards(enemy.transform.position, targetDestination, Speed * Time.deltaTime);

            if (y)
            {
                targetDestination = enemy.transform.position;
                targetDestination.y = currentDestination.position.y;
                enemy.transform.position =
                    Vector3.MoveTowards(enemy.transform.position, targetDestination, ySpeed * Time.deltaTime);
            }

            if (CheckDestination(enemy.transform.position, currentDestination.position, TranslationOffset, x, y, z))
            {
                StartCoroutine(ChangeDest());
            }

            yield return fixedUpdate;

        }

        AnimationBase.StopAnimation();
    }

    private IEnumerator ChangeDest()
    {
        if (currentDestIndex <= 0)
        {
            backDest = destinations[destinations.Count-1].position;
            forwardDest = destinations[currentDestIndex + 1].position;
        }
        else if (currentDestIndex >= destinations.Count - 1)
        {
            backDest = destinations[currentDestIndex - 1].position;
            forwardDest = destinations[0].position;
        }
        else
        {
            backDest = destinations[currentDestIndex - 1].position;
            forwardDest = destinations[currentDestIndex + 1].position;
        }

        /*if (forward)
        {
            if (GetDistance(forwardDest, player.transform.position) > DistanceFromPlayer)
            {
                currentDestIndex++;
                if (currentDestIndex >= destinations.Count)
                {
                    currentDestIndex = 0;
                }
            }
            else
            {
                forward = false;
                currentDestIndex--;
                if (currentDestIndex < 0)
                {
                    currentDestIndex = destinations.Count - 1;
                }
            }
        }
        else
        {
            if (GetDistance(backDest, player.transform.position) > DistanceFromPlayer)
            {
                currentDestIndex--;
                if (currentDestIndex < 0)
                {
                    currentDestIndex = destinations.Count - 1;
                }
            }
            else
            {
                forward = true;
                currentDestIndex++;
                if (currentDestIndex >= destinations.Count)
                {
                    currentDestIndex = 0;
                }
            }
        }*/

        if (forward && GetDistance(forwardDest, player.transform.position) > DistanceFromPlayer)
        {
            forward = true;
            currentDestIndex++;
            if (currentDestIndex >= destinations.Count)
            {
                currentDestIndex = 0;
            }
        }
        else if (!forward && GetDistance(backDest, player.transform.position) > DistanceFromPlayer)
        {
            forward = false;
            currentDestIndex--;
            if (currentDestIndex < 0)
            {
                currentDestIndex = destinations.Count - 1;
            }
        }
        else
        {
            if (GetDistance(forwardDest, player.transform.position) > GetDistance(backDest, player.transform.position))
            {
                forward = true;
                currentDestIndex++;
                if (currentDestIndex >= destinations.Count)
                {
                    currentDestIndex = 0;
                }
            }
            else
            {
                forward = false;
                currentDestIndex--;
                if (currentDestIndex < 0)
                {
                    currentDestIndex = destinations.Count - 1;
                }
            }
        }

        currentDestination = destinations[currentDestIndex];
        yield return new WaitForSeconds(Random.Range(minChangeDestTime, maxChangeDestTime));
    }
    
    private bool CheckDestination(Vector3 Dest01, Vector3 Dest02, float offset, bool x, bool y, bool z)
    {
        if ((!x || (Dest01.x >= Dest02.x - offset
             && Dest01.x <= Dest02.x + offset))
           
            &&(!z || (Dest01.z >= Dest02.z - offset
               && Dest01.z <= Dest02.z + offset)))
        {
            return true;
        }
        return false;
    }
    
    private float GetDistance(Vector3 dest01, Vector3 dest02)
    {
        return Mathf.Sqrt((Mathf.Pow((dest01.x - dest02.x), 2) + (Mathf.Pow((dest01.z - dest02.z), 2))));
    }
}
