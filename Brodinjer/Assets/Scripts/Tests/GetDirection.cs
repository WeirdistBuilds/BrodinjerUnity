using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDirection : MonoBehaviour
{
    public Transform check;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ReturnDirect(check);
        }
    }

    public virtual void ReturnDirect(Transform trans)
    {
        Vector3 collisionposition = trans.position;
        collisionposition.y = 0;
        Vector3 transformposition = transform.position;
        transformposition.y = 0;
        Vector3 target = trans.position - transform.position;
        float angle = Vector3.Angle(target, transform.forward);
    }
}
