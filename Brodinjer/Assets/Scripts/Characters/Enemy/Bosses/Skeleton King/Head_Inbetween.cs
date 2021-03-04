using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head_Inbetween : MonoBehaviour
{
    public Transform HeadRotate, LookRotate;
    public bool rotating;
    private float currentRotation = 0;
    public float RotationSpeed;
    public Vector3 Offset;

    private void Awake()
    {
        //Offset = HeadRotate.eulerAngles - transform.eulerAngles;
    }

    public void FixedUpdate()
    {
        if (rotating)
        {
            currentRotation += Time.deltaTime * RotationSpeed;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, LookRotate.eulerAngles + Offset, currentRotation);//RotationSpeed*Time.deltaTime);
        }
        else
        {
            currentRotation += Time.deltaTime * RotationSpeed;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, HeadRotate.eulerAngles + Offset, currentRotation);//RotationSpeed * Time.deltaTime);
        }
    }

    public void SetRotate(bool val)
    {
        currentRotation = 0;
        rotating = val;
    }
}
