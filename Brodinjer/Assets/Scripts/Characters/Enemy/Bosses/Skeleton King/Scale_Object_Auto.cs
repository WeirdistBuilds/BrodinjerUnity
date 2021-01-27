using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_Object_Auto : MonoBehaviour
{
    public float ScaleTime;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scalable"))
        {
            ScalableObjectBase scalable;
            if((scalable = other.GetComponent<ScalableObjectBase>())!=null)
            {
                scalable.AutoScale(ScaleTime, false);
            }
            else if((scalable = other.GetComponentInParent<ScalableObjectBase>()) != null)
            {
                scalable.AutoScale(ScaleTime, false);
            }
        }
    }
}
