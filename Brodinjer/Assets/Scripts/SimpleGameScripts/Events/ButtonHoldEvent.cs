using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ButtonHoldEvent : MonoBehaviour
{
    public UnityEvent KeyHoldEvent;
    public float holdTime;
    public bool ActiveOnStart;
    public string SkipButton;
    private bool active;
    private float currentTime;
    private Coroutine checkFunc;

    public Image SkipBar;

    private void Start()
    {
        if (ActiveOnStart)
        {
            setActive(true);
        }
        if (SkipBar != null)
            SkipBar.fillAmount = 0;
    }

    public void setActive(bool val)
    {
        if(val && checkFunc == null)
        {
            active = true;
            checkFunc = StartCoroutine(CheckInput());
        }
        else if (!val)
        {
            if (checkFunc != null)
                StopCoroutine(checkFunc);
            checkFunc = null;
            active = false;
        }
    }

    private IEnumerator CheckInput()
    {
        currentTime = 0;
        while (active)
        {
            while (Input.GetButton(SkipButton) && currentTime <= holdTime)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= holdTime)
                {
                    KeyHoldEvent.Invoke();
                }
                if (SkipBar != null)
                {
                    SkipBar.fillAmount = GeneralFunctions.ConvertRange(0, holdTime, 0, 1, currentTime);
                }
                yield return new WaitForFixedUpdate();
            }
            if (Input.GetButtonUp(SkipButton))
            {
                currentTime = 0;
                if (SkipBar != null)
                {
                    SkipBar.fillAmount = 0;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }




}
