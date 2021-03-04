using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Hand_Init_Scene : MonoBehaviour
{
    public UnityEvent endEvent;
    public Animator HandAnim, DoorAnim;
    public NavMeshAgent HandAgent;

    public GameObject SceneCam;// PlayerCam;

    public float InitWait, HandSurpriseWait, HandScurryTime, EndTimeWait;

    public Transform HandDestination;

    public float speedDif;

    private bool walking;
    public SoundController walkSound, doorsound;
    public float MaxFootstepInBetween, MinFootstepInBetween;

    public void StartScene()
    {
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        //PlayerCam.SetActive(false);
        yield return new WaitForSeconds(InitWait);
        HandAnim.SetTrigger("Surprise");
        yield return new WaitForSeconds(HandSurpriseWait);
        HandAgent.SetDestination(HandDestination.position);
        float currentTime = 0;
        walking = true;
        StartCoroutine(WalkSound());
        while (currentTime < HandScurryTime)
        {
            currentTime += Time.deltaTime;
            HandAnim.SetFloat("Speed", HandAgent.velocity.magnitude);
            HandAnim.SetFloat("Direction", GetDirection());
            HandAnim.speed = HandAgent.velocity.magnitude * speedDif;
            if (HandAnim.speed < 1)
            {
                HandAnim.speed = 1;
            }
            yield return new WaitForFixedUpdate();
        }
        walking = false;
        doorsound.Play();
        DoorAnim.SetTrigger("Close");
        yield return new WaitForSeconds(EndTimeWait);
        endEvent.Invoke();
        //PlayerCam.SetActive(true);
        SceneCam.SetActive(false);
    }
    
    public virtual float GetDirection()
    {
        float angle =  GeneralFunctions.GetDirection(HandAnim.velocity + HandAnim.transform.position, HandAnim.transform.position);
        angle /= 360;
        angle += .5f;
        return angle;
    }

    private IEnumerator WalkSound()
    {
        yield return new WaitForSeconds(.25f);
        while (walking)
        {
            walkSound.Play();
            yield return new WaitForSeconds(GeneralFunctions.ConvertRange(0, HandAgent.speed,
                MaxFootstepInBetween, MinFootstepInBetween, HandAgent.velocity.magnitude));
        }
    }

}
