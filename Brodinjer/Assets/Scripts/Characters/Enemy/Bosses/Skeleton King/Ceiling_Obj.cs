using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceiling_Obj : MonoBehaviour
{
    private Vector3 Scale;
    public float SpeedTime = 2;
    public float LifeTime;
    public GameObject colliderObj;

    public void SetScale(Vector3 scale)
    {
        Scale = scale;
    }

    public void Drop()
    {
        StartCoroutine(IncreaseScale());
    }

    private IEnumerator IncreaseScale()
    {
        float currentTime = 0;
        while(currentTime < SpeedTime)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, Scale, 
                GeneralFunctions.ConvertRange(0, SpeedTime, 0, 1, currentTime));
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != ToLayer(this.gameObject.layer))
        {
            this.gameObject.layer = 0;
            colliderObj.layer = 0;
            StartCoroutine(DestroyTime());
        }
    }

    private IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(this.gameObject);
    }

    public int ToLayer(int bitmask)
    {
        int result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1)
        {
            bitmask = bitmask >> 1;
            result++;
        }
        return result;
    }
}
