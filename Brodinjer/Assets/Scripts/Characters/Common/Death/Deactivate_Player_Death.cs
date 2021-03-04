using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character/Common/Death/Deactivate")]
public class Deactivate_Player_Death : Death_Base
{
    public float waitTime;
    
    public override IEnumerator Death()
    {
        yield return new WaitForSeconds(waitTime);
        character.GetComponent<PlayerMovement>().StopAll();
    }

    public override Death_Base GetClone()
    {
        Deactivate_Player_Death temp = CreateInstance<Deactivate_Player_Death>();
        temp.waitTime = waitTime;
        return temp;
    }

}
