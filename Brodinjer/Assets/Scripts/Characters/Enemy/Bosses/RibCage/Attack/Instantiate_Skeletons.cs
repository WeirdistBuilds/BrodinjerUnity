using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class Instantiate_Skeletons : MonoBehaviour
{
    public GameObject SkeletonPrefab;
    public GameObject HealthPrefab;
    public List<Transform> HealthInstantiateSpots;
    public List<Transform> InstantiateSpots;
    public int numSkeletons;
    private int numSkeletonsPresent;
    public IntData totalSkeletons;

    private List<Transform> spots;
    private int index;

    private bool instantiating;

    public Character_Manager RibcageManager;
    public UnityEvent endInstantiate;
    public void InstantiateSkeletons()
    {
        if (RibcageManager.dead)
            return;
        instantiating = false;
        numSkeletonsPresent = 0;
        Enemy_Manager[] enemies = FindObjectsOfType<Enemy_Manager>();
        foreach (var enemy in enemies)
        {
            if (!enemy.dead && enemy.isActiveAndEnabled)
                numSkeletonsPresent++;
        }

        numSkeletonsPresent--;
        
        if (numSkeletonsPresent < numSkeletons)
        {
            if (!instantiating)
            {
                instantiating = true;
                StartCoroutine(InstantiateObj());
            }
        }

        int randamount = Random.Range(1, 3);
        for (int i = 0; i < randamount; i++)
        {
            int randindex = Random.Range(0, HealthInstantiateSpots.Count);
            Instantiate(HealthPrefab, HealthInstantiateSpots[randindex].position, HealthPrefab.transform.rotation)
                .SetActive(true);
        }
    }

    private IEnumerator InstantiateObj()
    {
        spots = InstantiateSpots;
        for (int i = 0; i < numSkeletons - numSkeletonsPresent; i++)
        {
            index = Random.Range(0, spots.Count);
            GameObject temp = Instantiate(SkeletonPrefab, spots[index].position, SkeletonPrefab.transform.rotation);
            temp.SetActive(true);
            temp.name = "Skeleton: " + Random.Range(1, 9999);
            /*spots.RemoveAt(index);
            if (spots.Count <= 0)
                spots = InstantiateSpots;*/
            yield return new WaitForSeconds(.5f);
        }

        totalSkeletons.value = numSkeletons;
        instantiating = false;
        if (!RibcageManager.dead)
            endInstantiate.Invoke();
    }

}
