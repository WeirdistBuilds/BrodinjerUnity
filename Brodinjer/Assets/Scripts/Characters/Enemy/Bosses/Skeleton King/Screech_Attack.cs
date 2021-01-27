using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screech_Attack : Enemy_Attack_Base
{
    public string AnimationTriggerName;
    public BoxCollider CielingCollider;

    [System.Serializable]
    public class DestinationEntry
    {
        public Transform Dest01, Dest02;
    }

    public List<Ceiling_Obj> RandomCielingObjs;
    public List<DestinationEntry> destinations;
    private List<Ceiling_Obj> currentCeilingObjs;
    private List<List<float>> ceilingDests;
    public float minscale, maxscale;

    private void Start()
    {
        ceilingDests = new List<List<float>>();
        foreach(var dest in destinations)
        {
            List<float> tempfloats = new List<float>();
            float minx, miny, minz, maxx, maxy, maxz;
            if(dest.Dest01.position.x > dest.Dest02.position.x)
            {
                minx = dest.Dest02.position.x;
                maxx = dest.Dest01.position.x;
            }
            else
            {
                minx = dest.Dest01.position.x;
                maxx = dest.Dest02.position.x;
            }
            if (dest.Dest01.position.z > dest.Dest02.position.z)
            {
                minz = dest.Dest02.position.z;
                maxz = dest.Dest01.position.z;
            }
            else
            {
                minz = dest.Dest01.position.z;
                maxz = dest.Dest02.position.z;
            }
            if (dest.Dest01.position.y > dest.Dest02.position.y)
            {
                miny = dest.Dest02.position.y;
                maxy = dest.Dest01.position.y;
            }
            else
            {
                miny = dest.Dest01.position.y;
                maxy = dest.Dest02.position.y;
            }
            tempfloats.Add(minx);
            tempfloats.Add(maxx);
            tempfloats.Add(miny);
            tempfloats.Add(maxy);
            tempfloats.Add(minz);
            tempfloats.Add(maxz);
            ceilingDests.Add(tempfloats);
        }
        ResetObjs();

    }

    public override IEnumerator Attack()
    {
        animator.SetTrigger(AnimationTriggerName);
        yield return new WaitForSeconds(AttackStartTime);
        CielingCollider.isTrigger = true;
        foreach(var obj in currentCeilingObjs)
        {
            obj.gameObject.SetActive(true);
            obj.Drop();
        }
        yield return new WaitForSeconds(AttackActiveTime);
        CielingCollider.isTrigger = false;
        yield return new WaitForSeconds(CoolDownTime);
        ResetObjs();
        yield return null;
    }
    
    private void ResetObjs()
    {
        currentCeilingObjs = new List<Ceiling_Obj>();
        foreach(var dest in ceilingDests)
        {
            Ceiling_Obj randomObj = RandomCielingObjs[Random.Range(0, RandomCielingObjs.Count)];
            Vector3 tempPos;
            tempPos.x = Random.Range(dest[0], dest[1]);
            tempPos.y = Random.Range(dest[2], dest[3]);
            tempPos.z = Random.Range(dest[4], dest[5]);
            Vector3 scale = Vector3.one * Random.Range(minscale, maxscale);
            Ceiling_Obj tempObj = Instantiate(randomObj, tempPos, randomObj.transform.rotation);
            tempObj.transform.localScale = Vector3.zero;
            tempObj.SetScale(scale);
            currentCeilingObjs.Add(tempObj);
        }
    }
}
