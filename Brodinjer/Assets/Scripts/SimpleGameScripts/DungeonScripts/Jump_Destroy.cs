using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Jump_Destroy : MonoBehaviour
{
    public float minJumpVelocity;
    public int numJumps;
    private int currentJump;

    public List<UnityEvent> JumpEvents;
    public UnityEvent FinalJumpEvent;

    private float delayTime = .15f;
    private bool running;

    private void Awake()
    {
        currentJump = 0;
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
                if (!running && other.GetComponent<CharacterController>().velocity.y < minJumpVelocity)
                {
                    running = true;
                    JumpEvents[currentJump].Invoke();
                    currentJump++;
                    if (currentJump >= numJumps)
                    {
                        FinalJumpEvent.Invoke();
                    }
                    yield return new WaitForSeconds(delayTime);
                    running = false;
                }
        }
    }
}
