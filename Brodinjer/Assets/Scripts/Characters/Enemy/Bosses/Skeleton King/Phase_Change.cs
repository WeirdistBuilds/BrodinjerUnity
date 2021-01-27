using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase_Change : MonoBehaviour
{
    public List<Rigidbody> DropObjects;
    private int currentObj;
    private int count;

    public void Drop()
    {
        StartCoroutine(DropStalactites());
    }

    private IEnumerator DropStalactites()
    {
        count = DropObjects.Count;
        for(int i = 0; i < count; i++)
        {
            currentObj = Random.Range(0, DropObjects.Count);
            DropObjects[currentObj].isKinematic = false;
            DropObjects[currentObj].useGravity = true;
            DropObjects.RemoveAt(currentObj);
            yield return new WaitForSeconds(Random.Range(0f, .5f));
        }
    }

}
