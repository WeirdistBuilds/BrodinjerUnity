using System.Collections;
using UnityEngine;

public class Skeleton_Armor_Setup : MonoBehaviour
{
    public bool LargeSkeleton = false;
    public bool MediumSkeleton = false;
    public bool Randomized = true;
    public GameObject LowArmor, MediumArmor, HighArmor;
    public Enemy_Character_Manager enemy;

    private void Start()
    {
        LowArmor.SetActive(false);
        MediumArmor.SetActive(false);
        HighArmor.SetActive(false);
        StartCoroutine(Setup());
    }

    private IEnumerator Setup()
    {
        yield return new WaitForSeconds(.1f);
        if (LargeSkeleton)
        {
            HighArmor.SetActive(true);
            enemy.Character.Health.TotalHealth = 50;
            LimitFloatData health = enemy.Character.Health.health as LimitFloatData;
            health.MaxValue = 50;
            enemy.Character.Health.health.value = 50;
        }
        else if(Randomized)
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                LowArmor.SetActive(true);
                enemy.Character.Health.TotalHealth = 15;
                LimitFloatData health = enemy.Character.Health.health as LimitFloatData;
                health.MaxValue = 15;
                enemy.Character.Health.health.value = 15;

            }
            else
            {
                MediumArmor.SetActive(true);
                enemy.Character.Health.TotalHealth = 30;
                LimitFloatData health = enemy.Character.Health.health as LimitFloatData;
                health.MaxValue = 30;
                enemy.Character.Health.health.value = 30;

            }
        }
        else
        {
            if (MediumSkeleton)
            {
                MediumArmor.SetActive(true);
                enemy.Character.Health.TotalHealth = 30;
                enemy.Character.Health.health.value = 30;

            }
            else
            {
                LowArmor.SetActive(true);
                enemy.Character.Health.TotalHealth = 15;
                enemy.Character.Health.health.value = 15;

            }
        }
    }

}
