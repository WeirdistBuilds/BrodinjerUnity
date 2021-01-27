using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScalableObjectBase : MonoBehaviour
{
    public Scaling_VFX highlightFX;

    public abstract void SetInit(float scale);
    public abstract void SetMax();
    public abstract void ScaleUp(bool deltaTimed);
    public abstract void ScaleDown(bool deltaTimed);
    public abstract void AutoScale(float time, bool Up);
}
