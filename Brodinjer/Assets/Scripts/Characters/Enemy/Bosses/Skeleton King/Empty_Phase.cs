using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty_Phase : Phase_Base
{
    public override IEnumerator RunPhase()
    {
        yield return null;
    }

    public override void StartPhase()
    {
    }

    public override void StopAttack()
    {
    }

    public override void StopDamage()
    {
    }

    public override void StopPhase()
    {
        
    }
}
