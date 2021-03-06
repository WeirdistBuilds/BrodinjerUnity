using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    public float FadeOutTime, FadeInTime, BlackoutTime;
    public UnityEvent FadeEvent;
    private bool running;
    public Image blackImage;
    private float currentTime;
    public bool startFaded;
    public bool main;

    private void Start()
    {
        running = false;
        if (main)
        {
            if (!startFaded)
                blackImage.color = Color.clear;
            else
                blackImage.color = Color.black;
        }
    }

    public void StartFade()
    {
        if (!running)
        {
            running = true;
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade()
    {
        currentTime = 0;
        while (currentTime < FadeOutTime)
        {
            currentTime += Time.deltaTime;
            blackImage.color = Color.Lerp(Color.clear, Color.black,
                GeneralFunctions.ConvertRange(0, FadeOutTime, 0, 1, currentTime));
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForFixedUpdate();
        FadeEvent.Invoke();
        yield return new WaitForSeconds(BlackoutTime);
        currentTime = 0;
        while (currentTime < FadeInTime)
        {
            currentTime += Time.deltaTime;
            blackImage.color = Color.Lerp(Color.black, Color.clear,
                GeneralFunctions.ConvertRange(0, FadeInTime, 0, 1, currentTime));
            yield return new WaitForFixedUpdate();
        }
       running = false;
    }

    public void StartFadeIn()
    {
        if (!running)
        {
            running = true;
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        FadeEvent.Invoke();
        yield return new WaitForSeconds(BlackoutTime);
        currentTime = 0;
        while (currentTime < FadeInTime)
        {
            currentTime += Time.deltaTime;
            blackImage.color = Color.Lerp(Color.black, Color.clear,
                GeneralFunctions.ConvertRange(0, FadeInTime, 0, 1, currentTime));
            yield return new WaitForFixedUpdate();
        }
        running = false;
    }

   
    
}
