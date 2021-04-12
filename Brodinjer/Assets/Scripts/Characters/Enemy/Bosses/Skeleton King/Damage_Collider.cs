using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damage_Collider : MonoBehaviour
{
    public Boss_Character_Manager bossManager;
    public LayerMask DamageLayer;
    public float ArmorAmount;
    public UnityEvent DamageEvent;
    public float damageDelay;

    private IEnumerator OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == ToLayer(DamageLayer.value))
        {
            Debug.Log("HIT");
            WeaponDamageAmount temp = coll.GetComponent<WeaponDamageAmount>();
            if (temp != null)
            {
                Debug.Log("NOT NULL");
                if (!temp.SingleHit || (temp.SingleHit && !temp.hit))
                {
                    temp.hit = true;
                    yield return new WaitForSeconds(damageDelay);
                    if (temp.DamageAnimationTrigger != "")
                    {
                        bossManager.TakeDamage(temp.DamageAmount, temp.DecreasedbyArmor, ArmorAmount, temp.DamageAnimationTrigger);
                    }
                    else
                    {
                        bossManager.TakeDamage(temp.DamageAmount, temp.DecreasedbyArmor, ArmorAmount);
                    }
                    yield return new WaitForSeconds(.1f);
                    if (bossManager.health.health.value > 0)
                    {
                        DamageEvent.Invoke();
                    }
                }
            }
        }
    }
    
    public int ToLayer (int bitmask ) {
        int result = bitmask>0 ? 0 : 31;
        while( bitmask>1 ) {
            bitmask = bitmask>>1;
            result++;
        }
        return result;
    }
}
