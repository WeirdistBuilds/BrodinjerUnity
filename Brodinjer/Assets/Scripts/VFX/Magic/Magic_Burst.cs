using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Magic_Burst : MonoBehaviour
{
    public GameObject BurstPrefab;
    public GameObject ProjectilePrefab;

    private Vector3 position;
    private Quaternion rotation;

    public UnityEvent onHit;

    public string IgnoreTag = "Player";
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag(IgnoreTag))
        {
            position = ProjectilePrefab.transform.position;
            rotation = ProjectilePrefab.transform.rotation;
            if (BurstPrefab != null)
            {
                BurstPrefab.SetActive(true);
                BurstPrefab.transform.parent = null;
            }
            onHit.Invoke();
            //ProjectilePrefab.SetActive(false);
            //GameObject temp = Instantiate(BurstPrefab, position, rotation);
           //temp.SetActive(true);
        }
    }
}
