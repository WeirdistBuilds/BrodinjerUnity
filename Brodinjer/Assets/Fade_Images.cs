using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class Fade_Images : MonoBehaviour
{
    public List<Graphic> images;
    public float FadeInTime, ActiveTime, FadeOutTime;
    private Color FullColor, ClearColor, CurrentColor;
    public float InitWaitTime;
    private bool running;
    public UnityEvent endTutorialEvent;

    private void OnEnable()
    {
        FullColor = new Color(255, 255, 255, 1);
        ClearColor = new Color(255, 255, 255, 0);
        CurrentColor = ClearColor;
        foreach(var img in images)
        {
            img.color = ClearColor;
        }
        Activate();
    }

    public void Activate()
    {
        StartCoroutine(TimeActive());
    }

    private IEnumerator TimeActive()
    {
        yield return new WaitForSeconds(InitWaitTime);
        float currentTime = 0;
        while(currentTime < FadeInTime)
        {
            currentTime += Time.deltaTime;
            CurrentColor = Color.Lerp(ClearColor, FullColor, GeneralFunctions.ConvertRange(0, FadeInTime, 0, 1, currentTime));
            foreach (var img in images)
            {
                img.color = CurrentColor;
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(ActiveTime);
        currentTime = 0;
        while(currentTime < FadeOutTime)
        {
            currentTime += Time.deltaTime;
            CurrentColor = Color.Lerp(FullColor, ClearColor, GeneralFunctions.ConvertRange(0, FadeOutTime, 0, 1, currentTime));
            foreach (var img in images)
            {
                img.color = CurrentColor;
            }
            yield return new WaitForFixedUpdate();
        }
        endTutorialEvent.Invoke();
        gameObject.SetActive(false);
    }

}
