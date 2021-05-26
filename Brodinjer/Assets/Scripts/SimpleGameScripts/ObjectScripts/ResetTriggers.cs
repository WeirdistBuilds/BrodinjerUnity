using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTriggers : MonoBehaviour
{
    public Animator anim;
    public List<string> triggerNames;
    public List<string> boolNames;
    public List<string> floatNames;
    public List<string> intNames;

    public void ResetAllTriggers()
    {
        foreach (var triggername in triggerNames)
        {
            anim.ResetTrigger(triggername);
        }
    }

    public void SetBoolFalse(string boolName)
    {
        anim.SetBool(boolName, false);
    }

    public void SetBoolTrue(string boolName)
    {
        anim.SetBool(boolName, true);
    }

    public void ResetAll()
    {
        foreach(var a in triggerNames)
        {
            anim.ResetTrigger(a);
        }
        foreach(var a in boolNames)
        {
            anim.SetBool(a, false);
        }
        foreach(var a in floatNames)
        {
            anim.SetFloat(a, 0);
        }
        foreach(var a in intNames)
        {
            anim.SetInteger(a, 0);
        }
    }
}
