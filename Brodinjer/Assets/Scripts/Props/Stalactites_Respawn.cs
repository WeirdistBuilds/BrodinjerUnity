using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactites_Respawn : MonoBehaviour
{
    public float InstantiateWaitTime;
    public void CreateNew(GameObject obj)
    {
        StartCoroutine(InstantiateObj(obj));
    }

    private IEnumerator InstantiateObj(GameObject obj)
    {
        yield return new WaitForSeconds(InstantiateWaitTime);
        Instantiate(obj);
    }
}
