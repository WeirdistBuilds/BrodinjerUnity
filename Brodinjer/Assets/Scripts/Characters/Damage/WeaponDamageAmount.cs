using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageAmount : MonoBehaviour
{
    public float DamageAmount;
    public bool DecreasedbyArmor;

    public float StunTime;

    public bool SingleHit = false;
    [HideInInspector]
    public bool hit = false;

    public float KnockbackForce, KnockbackTime;

    public GameObject BaseObj;

    public Vector3 knockbackDirection;

    public string DamageAnimationTrigger;

    public float RecoveryTime;
    public float singleHitTimer = -1;

    public void SetKnockbackDirection(Vector3 newDirection)
    {
        knockbackDirection = newDirection;
    }

    private void Start()
    {
        if(BaseObj == null)
        {
            BaseObj = this.gameObject;
        }
        hit = false;
    }

    private void OnDisable()
    {
        hit = false;
        StopAllCoroutines();
    }

    public IEnumerator SingelHitTimer()
    {
        yield return new WaitForSeconds(singleHitTimer);
        hit = false;
    }
}
