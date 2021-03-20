using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Image barUI;
    private List<LimitFloatData> HealthAmount = new List<LimitFloatData>();
    private float currentHealth;
    private float maxHealth;
    private bool running;
    public GameObject healthobj;

    public void StartHealthCheck()
    {
        if (HealthAmount.Count == 1)
            maxHealth = HealthAmount[0].MaxValue;
        else
        {
            maxHealth = 0;
            foreach (var health in HealthAmount)
            {
                maxHealth += health.MaxValue;
            }
        }
        running = true;
        healthobj.SetActive(true);
    }

    public void AddHealth(LimitFloatData health)
    {
        HealthAmount.Add(health);
    }

    void FixedUpdate()
    {
        if (running) {
            if (HealthAmount.Count == 1)
                currentHealth = HealthAmount[0].value;
            else
            {
                currentHealth = 0;
                foreach (var health in HealthAmount)
                    currentHealth += health.value;
            }
            barUI.fillAmount = currentHealth / maxHealth;
            if(currentHealth <= 0)
            {
                healthobj.SetActive(false);
                running = false;
            }

        }
    }

    public void ResetHealths()
    {
        HealthAmount = new List<LimitFloatData>();
    }
}
