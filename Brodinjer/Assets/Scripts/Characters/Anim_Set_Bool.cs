using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Set_Bool : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetTrue(string animString)
    {
        anim.SetBool(animString, true);
    }

    public void SetFalse(string animString)
    {
        anim.SetBool(animString, false);
    }
}
