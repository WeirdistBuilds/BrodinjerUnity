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

    public float InitWaitTime;
    public SoundController sound;

    private void OnEnable()
    {
        foreach (var img in images)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a);
        }
    }

    public void Activate(string text)
    {
        indicatorText.text = text;
        StartCoroutine(TimeActive());
    }

    private IEnumerator TimeActive()
    {
        yield return new WaitForSeconds(InitWaitTime);
        sound.Play();
        float currentTime = 0;
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
    }
}
