using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head_Follow : MonoBehaviour
{
    public Transform FollowRotation;
    public Transform R_Brow, L_Brow;
    public Transform RBrowPosition, LBrowPosition;
    public bool Rotating;
    //public List<Transform> destinations;
    public Head_Inbetween headRotate;

    private void LateUpdate()
    {
        if (Rotating)
        {
            //transform.rotation = FollowRotation.rotation;
            transform.rotation = FollowRotation.rotation;
            R_Brow.position = RBrowPosition.position;
            R_Brow.rotation = RBrowPosition.rotation;
            L_Brow.position = LBrowPosition.position;
            L_Brow.rotation = LBrowPosition.rotation;
        }
    }

    public void SetRotate(bool val)
    {
        headRotate.SetRotate(true);
        Rotating = val;
    }
}
