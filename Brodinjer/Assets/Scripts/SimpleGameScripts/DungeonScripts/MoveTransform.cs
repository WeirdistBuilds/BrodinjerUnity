using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransform : MonoBehaviour
{
    public Transform ObjToMove;
    private CharacterController _cc;

    private void Awake()
    {
        _cc = ObjToMove.GetComponent<CharacterController>();
        if (_cc == null)
        {
            _cc=ObjToMove.GetComponentInChildren<CharacterController>();
        }
    }

    public void Move(Transform PlaceToMoveTo)
    {
        if (_cc == null)
            _cc = ObjToMove.GetComponentInChildren<CharacterController>();
        if (_cc != null)
            _cc.enabled = false;
        ObjToMove.transform.position = PlaceToMoveTo.transform.position;
        ObjToMove.rotation = PlaceToMoveTo.rotation;
        if (_cc != null)
            _cc.enabled = true;
    }

    public void MovePosition(Vector3Data positionData)
    {
        if (_cc != null)
        {
            _cc.enabled = false;
        }

        ObjToMove.transform.position = positionData.vector;

        if (_cc != null)
        {
            _cc.enabled = true;
        }
    }

    public void MoveRotation(Vector3Data rotationData)
    {
        if (_cc != null)
        {
            _cc.enabled = false;
        }
        ObjToMove.rotation = Quaternion.Euler(rotationData.vector);
        if (_cc != null)
            _cc.enabled = true;
    }
}
