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
    public float RotationSpeed;

    private void LateUpdate()
    {
        if (Rotating)
        {
            //transform.rotation = FollowRotation.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, FollowRotation.rotation, RotationSpeed * Time.deltaTime);
            R_Brow.position = RBrowPosition.position;
            R_Brow.rotation = RBrowPosition.rotation;
            L_Brow.position = LBrowPosition.position;
            L_Brow.rotation = LBrowPosition.rotation;
        }
    }

    public void SetRotate(bool val)
    {
        Rotating = val;
    }
}
