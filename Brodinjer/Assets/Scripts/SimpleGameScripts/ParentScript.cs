using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentScript : MonoBehaviour
{
    public void ParentTo(Transform objectToParent)
    {
        objectToParent.parent = this.transform;
    }
}
