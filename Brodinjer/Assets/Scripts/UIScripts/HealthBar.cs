using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image barUI;
    public LimitFloatData HealthAmount;
    public float increaseSpeed;
    public bool autoRegen;
    
    void Start()
    {
        barUI = GetComponent<Image>();
    }

    public void SetHealthRegen(float num)
    {
        if (num > 0)
            autoRegen = true;
        else
            autoRegen = false;
        increaseSpeed = num;
    }

    void FixedUpdate()
    {
        if (HealthAmount.value
            <= HealthAmount.MaxValue
            && HealthAmount.value > 0.0f
            && autoRegen)
        { 
            HealthAmount.AddFloat(increaseSpeed * Time.deltaTime);
        }
        
        barUI.fillAmount = HealthAmount.value / HealthAmount.MaxValue;
    }
}
