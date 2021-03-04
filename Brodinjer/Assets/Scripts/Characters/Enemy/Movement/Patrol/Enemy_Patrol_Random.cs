using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Patrol_Random : Enemy_Patrol
{
    public bool useX, useY, useZ;
    private Vector3 randomVector;

    private float minx, maxx, miny, maxy, minz, maxz;

    public void FindMinMax()
    {
        if (destinations.Count == 0)
        {
            minx = enemy.transform.position.x;
            maxx = minx;
            miny = enemy.transform.position.y;
            maxy = miny;
            minz = enemy.transform.position.z;
            maxz = minz;
        }
        else if (destinations.Count > 0)
        {
            minx = destinations[0].transform.position.x;
            maxx = minx;
            miny = destinations[0].transform.position.y;
            maxy = miny;
            minz = destinations[0].transform.position.z;
            maxz = minz;

            if (destinations.Count > 1)
            {
                for (int i = 1; i < destinations.Count; i++)
                {
                    if (destinations[i].position.x > maxx)
                    {
                        maxx = destinations[i].position.x;
                    }
                    else if (destinations[i].position.x < minx)
                    {
                        minx = destinations[i].position.x;
                    }
                    if (destinations[i].position.y > maxy)
                    {
                        maxy = destinations[i].position.y;
                    }
                    else if (destinations[i].position.y < miny)
                    {
                        miny = destinations[i].position.y;
                    }
                    if (destinations[i].position.z > maxz)
                    {
                        maxz = destinations[i].position.x;
                    }
                    else if (destinations[i].position.z < minz)
                    {
                        minz = destinations[i].position.x;
                    }
                }
            }
        }

    }
      
    protected override void Init()
    {
        base.Init(); 
        FindMinMax();
        randomVector = enemy.transform.position;
        if(useX)
            randomVector.x = Random.Range(minx, maxx);
        if(useY)
            randomVector.y = Random.Range(miny, maxy);
        if(useZ)
            randomVector.z = Random.Range(minz, maxz);
        agent.destination = randomVector;
    }

    public override IEnumerator Move()
    {
        agent.speed = Speed;
        agent.updateRotation = true;
        while (moving)
        {
            if (CheckPosition(randomVector))
            {
                StartCoroutine(ChangeDest());
            }

            yield return new WaitForSeconds(.1f);
            
        }
    }


    public override IEnumerator ChangeDest()
    {
        agent.speed = 0;
        yield return new WaitForSeconds(ChangeTime());
        if(useX)
            randomVector.x = Random.Range(minx, maxx);
        if(useY)
            randomVector.y = Random.Range(miny, maxy);
        if(useZ)
            randomVector.z = Random.Range(minz, maxz);
        agent.destination = randomVector;
        agent.speed = Speed;
    }
}
