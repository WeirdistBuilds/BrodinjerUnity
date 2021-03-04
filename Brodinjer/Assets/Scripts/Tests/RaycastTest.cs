using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour
{

    public GameObject objectToMove;
    private RaycastHit hit;
    public float startDistance;
    public float distance;
    public bool DebugObj = false;
    
    private void FixedUpdate()
    {
        if(DebugObj)
            Debug.DrawLine(transform.position- (transform.forward*20), transform.position + (transform.forward*20), Color.red, 1);
        if (Physics.Raycast(transform.position + (transform.forward*startDistance), transform.forward, out hit, distance))
        {
            if(hit.collider.tag != "Player")
                objectToMove.transform.position = hit.point;
            else
                objectToMove.transform.position = transform.position + transform.forward * 100;
        }
        else
        {
            objectToMove.transform.position = transform.position + transform.forward * 100;
        }
    }
}
