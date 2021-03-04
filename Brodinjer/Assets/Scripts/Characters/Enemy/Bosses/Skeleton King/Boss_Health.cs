using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Common/Boss Health")]
public class Boss_Health : Health_Base
{
    public void TakeDamage(float amount, bool armor, float ArmorAmount)
    {
        DecreaseHealth(amount, armor, ArmorAmount);
    }

    public void DecreaseHealth(float amount, bool armor, float ArmorDecreaseAmount)
    {
        if (armor)
        {
            float decreaseAmount = amount - ArmorDecreaseAmount;
            if (decreaseAmount < 0)
                decreaseAmount = 0;
            health.SubFloat(decreaseAmount);
        }
        else
        {
            health.SubFloat(amount);
        }
        if (health.value <= 0)
        {
            Death();
        }
    }
}
