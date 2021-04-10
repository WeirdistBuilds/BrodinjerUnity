using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMovement : MonoBehaviour
{
    public CharacterController controller;
    public float JumpBackDistance, JumpTime;
    public string JumpBackString;
    public Animator anim;
    public float RotationSpeed;
    public float RunSpeed;

    public float TurnTime;
    public Transform destination;
    public GameObject geoObject;

    public void StopMove()
    {
        StopAllCoroutines();
        anim.SetTrigger("Idle");
        anim.SetFloat("Speed", 0);
    }

    public void ForceMoveTowards(Transform destination)
    {
        geoObject.gameObject.SetActive(false);
        StartCoroutine(waitTime(.1f));

    }

    IEnumerator waitTime(float waittime)
    {
        yield return new WaitForSeconds(.1f);
        controller.transform.position = Vector3.MoveTowards(controller.transform.position, destination.position, 5);
        yield return new WaitForSeconds(waittime);
        geoObject.gameObject.SetActive(true);
    }

    public void JumpBack(Transform direction)
    {
        StartCoroutine(Dodge(destination, JumpBackDistance, JumpTime, JumpBackString));
    }
    private IEnumerator Dodge(Transform dodgeDirection, float amount, float dodgeTime, string animationString)
    {
        float currentTime = 0;
        if (animationString != "")
        {
            anim.speed = 1;
            anim.gameObject.GetComponent<ResetTriggers>().ResetAllTriggers();
            anim.SetTrigger(animationString);
        }
        while (currentTime < dodgeTime)
        {
            Vector3 targetRotation = dodgeDirection.position;
            targetRotation = (targetRotation - controller.transform.position).normalized;
            Quaternion facingDirection = Quaternion.LookRotation(targetRotation);
            Quaternion YRotation = Quaternion.Euler(controller.transform.rotation.eulerAngles.x,
                facingDirection.eulerAngles.y, controller.transform.rotation.eulerAngles.z);
            if (!GeneralFunctions.CheckDestination(controller.transform.rotation.eulerAngles,
                YRotation.eulerAngles, .1f))
            {
                controller.transform.rotation =
                    Quaternion.Lerp(controller.transform.rotation, YRotation, RotationSpeed * Time.deltaTime);
            }
            currentTime += Time.deltaTime;
            Vector3 targetDestination = controller.transform.position + (controller.transform.forward * RunSpeed * Time.deltaTime);
            targetDestination.y = controller.transform.position.y;
            controller.transform.position =
                Vector3.MoveTowards(controller.transform.position, targetDestination, RunSpeed * Time.deltaTime);

            yield return new WaitForFixedUpdate();

        }
        anim.SetTrigger("Idle");
    }
    public void TurnAround(Transform direction)
    {
        StartCoroutine(Turn(direction, TurnTime));
    }

    private IEnumerator Turn(Transform direction, float turnTime)
    {
        float currentTime = 0;
        Quaternion origRotation = controller.transform.rotation;
        while(currentTime < turnTime)
        {
            currentTime += Time.deltaTime;
            controller.transform.rotation = Quaternion.Lerp(origRotation, direction.rotation, GeneralFunctions.ConvertRange(0, turnTime, 0, 1, currentTime));
            yield return new WaitForFixedUpdate();
        }
    }
}
