using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Randomize_Rib_Attack : MonoBehaviour
{
    public UnityEvent Attack01, Attack02;

    private float percentage01 = .5f;

    private int randomNum;

    private int currentNum = -1;

    public void RunRandomEvent()
    {
        randomNum = Random.Range(0, 11);
        if ((randomNum/10.0f) < percentage01)
        {
            Attack01.Invoke();
            if (currentNum == 0)
                percentage01 -= .35f;
            else
                percentage01 = .5f;
            currentNum = 0;
        }
        else
        {
            Attack02.Invoke();
            if (currentNum == 1)
                percentage01 += .35f;
            else
                percentage01 = .5f;
            currentNum = 1;
        }
    }
}
