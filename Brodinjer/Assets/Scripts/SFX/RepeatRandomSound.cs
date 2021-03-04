using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatRandomSound : MonoBehaviour
{
    public SoundController sounds;
    public float inbetweenTime;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.25f);
        StartCoroutine(run());
    }

    private IEnumerator run()
    {
        while (true)
        {
            sounds.Play();
            yield return new WaitForSeconds(inbetweenTime);
        }
    }
}
