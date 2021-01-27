using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Phase_Base : MonoBehaviour
{
    public UnityEvent StartPhaseEvent;
    public abstract void StartPhase();
    public abstract IEnumerator RunPhase();
    public abstract void StopPhase();
    protected bool currentPhase;
    protected Coroutine phaseFunc;
    public abstract void StopAttack();
    public abstract void StopDamage();

}
