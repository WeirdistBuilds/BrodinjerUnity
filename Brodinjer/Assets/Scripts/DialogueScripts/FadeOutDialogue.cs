using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutDialogue : MonoBehaviour
{
    public List<Graphic> images;
    public float FadeInTime, ActiveTime, FadeOutTime;
    private Color ClearColor;
    private List<Color> FullColor = new List<Color>();
    public float InitWaitTime;

    private void OnEnable()
    {
        FullColor = new List<Color>();
        ClearColor = new Color(0, 0, 0, 0);
        for (int i = 0; i<images.Count; i++)
        {
            FullColor.Add(images[i].color);
            images[i].color = ClearColor;
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
        while (currentTime < FadeInTime)
        {
            currentTime += Time.deltaTime;
            for(int i = 0; i<images.Count; i++)
            {
                images[i].color = Color.Lerp(ClearColor, FullColor[i], GeneralFunctions.ConvertRange(0, FadeInTime, 0, 1, currentTime));
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
                images[i].color = Color.Lerp(FullColor[i], ClearColor, GeneralFunctions.ConvertRange(0, FadeInTime, 0, 1, currentTime));
            }
            yield return new WaitForFixedUpdate();
        }
        gameObject.SetActive(false);
    }
}
