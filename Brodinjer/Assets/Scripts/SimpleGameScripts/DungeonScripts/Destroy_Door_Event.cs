using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destroy_Door_Event : MonoBehaviour
{
    public string TagName;
    public int NumHits;
    private int currentHit;
    public List<UnityEvent> hitEvents;
    public float betweenHitTime;
    private bool running;

    private void Awake()
    {
        currentHit = 0;
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName))
        {
            if (!running && currentHit < NumHits)
            {
                running = true;
                currentHit++;
                if (hitEvents.Count > currentHit)
                {
                    hitEvents[currentHit].Invoke();
                }
                yield return new WaitForSeconds(betweenHitTime);
                running = false;
            }
            
        }
    }
}
