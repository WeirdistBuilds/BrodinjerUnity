using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head_Rotation_Set : MonoBehaviour
{
    public Animator anim;
    public float MinX, MaxX, MinY, MaxY, MinZ, MaxZ;
    private float X, Y, Z;
    public Transform FollowObj;
    public BoolData Side;

    private void Start()
    {
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            if (!Side.value)
            {
                X = GeneralFunctions.ConvertRange(MinX, MaxX, -1, 1, FollowObj.position.x);
                Y = GeneralFunctions.ConvertRange(MinY, MaxY, -1, 1, FollowObj.position.y);
                Z = GeneralFunctions.ConvertRange(MinZ, MaxZ, -1, 1, FollowObj.position.z);
                
            }
            else
            {
                X = GeneralFunctions.ConvertRange(MinX, MaxX, 1, -1, FollowObj.position.x);
                Y = GeneralFunctions.ConvertRange(MinY, MaxY, -1, 1, FollowObj.position.y);
                Z = GeneralFunctions.ConvertRange(MinZ, MaxZ, -1, 1, FollowObj.position.z);
            }
            anim.SetFloat("X", X);
            anim.SetFloat("Y", Y);
            anim.SetFloat("Z", Z);
            yield return new WaitForFixedUpdate();

        }
    }

}
