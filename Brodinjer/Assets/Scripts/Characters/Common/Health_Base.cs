using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character/Common/Health")]
public class Health_Base : ScriptableObject
{
    public float TotalHealth;

    public float ArmorDamageDecrease;

    public Death_Base Death_Version;
    private Death_Base _death;

    protected MonoBehaviour caller;

    public FloatData health;
    private FloatData _health;

    private bool dead = false;

    public virtual void Init(MonoBehaviour caller, Transform enemy, bool mainCharacter = false)
    {
        dead = false;
        TotalHealth = health.value;
        if (!mainCharacter)
        {
            _death = Death_Version.GetClone();
            Death_Version = _death;
            _health = health.GetClone();
            health = _health;
        }

        Death_Version.Init(enemy);
        this.caller = caller;
    }

    public virtual void Death()
    {
        if (!dead)
        {
            dead = true;
            caller.StartCoroutine(Death_Version.Death());
        }
    }
    
    public virtual void TakeDamage(float amount, bool armor)
    {
        DecreaseHealth(amount, armor);
    }

    public virtual void TakeDamage(FloatData amount, bool armor)
    {
        DecreaseHealth(amount.value, armor);
    }

    public virtual void DecreaseHealth(float amount, bool armor)
    {
        if (armor)
        {
            float decreaseAmount = amount - ArmorDamageDecrease;
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

    public float GetHealth()
    {
        return health.value;
    }

    public virtual Health_Base GetClone()
    {
        Health_Base temp = ScriptableObject.CreateInstance<Health_Base>();
        temp.Death_Version = Death_Version;
        temp.TotalHealth = TotalHealth;
        temp.ArmorDamageDecrease = ArmorDamageDecrease;
        temp.health = health;
        return temp;
    }
}
