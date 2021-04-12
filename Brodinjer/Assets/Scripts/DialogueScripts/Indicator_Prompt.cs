using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Indicator_Prompt : MonoBehaviour
{
    public TextMeshProUGUI indicatorText;
    public List<Graphic> images;
    public float FadeInTime, ActiveTime, FadeOutTime;
    public List<float> fullalphas;
    float currentTime = 0;

    public float InitWaitTime;

    private bool running = false;
    private bool waiting = false;
    private string nextText;

    private void OnEnable()
    {
        running = false;
        foreach (var img in images)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a);
        }
    }

    public void Activate(string text)
    {
        if (!running)
        {
            indicatorText.text = text;
            running = true;
            StartCoroutine(TimeActive());
        }
        else
        {
            waiting = true;
            nextText = text;
        }
    }

    private IEnumerator TimeActive()
    {
        yield return new WaitForSeconds(InitWaitTime);
        running = true;
        currentTime = 0;
        while (currentTime < FadeInTime)
        {
            currentTime += Time.deltaTime;
            for(int i = 0; i < images.Count; i++)
            {
                Graphic img = images[i];
                img.color = new Color(img.color.r, img.color.g, img.color.b, GeneralFunctions.ConvertRange(0, FadeInTime, 0, fullalphas[i], currentTime));
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(ActiveTime);
        currentTime = 0;
        while (currentTime < FadeOutTime)
        {
            currentTime += Time.deltaTime;
            for (int i = 0; i < images.Count; i++)
            {
                Graphic img = images[i];
                img.color = new Color(img.color.r, img.color.g, img.color.b, GeneralFunctions.ConvertRange(0, FadeOutTime, fullalphas[i], 0, currentTime));
            }
            yield return new WaitForFixedUpdate();
        }
        if (waiting)
        {
            waiting = false;
            indicatorText.text = nextText;
            StartCoroutine(TimeActive());
        }
        running = false;
    }
}
