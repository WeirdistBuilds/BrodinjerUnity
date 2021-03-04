using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character/Common/Death/Destroy")]
public class Destroy_Death : Death_Base
{
    public float waitTime=0;
    public override IEnumerator Death()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(character.gameObject);
    }

    public override Death_Base GetClone()
    {
        Destroy_Death temp = CreateInstance<Destroy_Death>();
        temp.waitTime = waitTime;
        return temp;
    }
}
