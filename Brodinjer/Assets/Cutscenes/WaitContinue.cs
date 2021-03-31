using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WaitContinue : MonoBehaviour
{
    private bool waiting;
    public Dialogue_Manager DialogueManager;
    public PlayableDirector playableDirector;
    public GameObject InteractButton;

    private void FixedUpdate()
    {
        if (waiting)
        {
            if (Input.GetButton("Interact"))
            {
                waiting = false;
                DialogueManager.NextLine();
                playableDirector.Play();
                InteractButton.SetActive(false);
            }
        }
    }

    public void Wait()
    {
        playableDirector.Pause();
        waiting = true;
        InteractButton.SetActive(true);
    }
}
