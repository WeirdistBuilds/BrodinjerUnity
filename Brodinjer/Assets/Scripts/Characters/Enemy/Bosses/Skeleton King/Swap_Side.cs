using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Swap_Side : MonoBehaviour
{
    public Transform Side01, Side02;
    private bool side1 = true;
    public float WaitTime;
    public UnityEvent SwapEvent;

    private void Awake()
    {
        side1 = true;
    }

    public void SwapSide()
    {
        StartCoroutine(Swap());
    }

    private IEnumerator Swap()
    {
        yield return new WaitForSeconds(WaitTime);
        if (side1)
        {
            transform.position = Side02.position;
            transform.rotation = Side02.rotation;
            side1 = false;
        }
        else
        {
            transform.position = Side01.position;
            transform.rotation = Side01.rotation;
            side1 = true;
        }
        SwapEvent.Invoke();
    }
}
