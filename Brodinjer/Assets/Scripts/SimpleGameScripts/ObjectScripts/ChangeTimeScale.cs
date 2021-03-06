using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTimeScale : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1;
    }

    public void SetScale(float scale)
    {
        Time.timeScale = scale;
    }
}
