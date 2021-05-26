using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCallBacks : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        transform.parent.gameObject.SetActive(false);       
    }
}
