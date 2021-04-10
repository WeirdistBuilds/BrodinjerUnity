using System.Collections;
using UnityEngine;

public class ScalableObject : ScalableObjectBase
{
    public float MinScaleMultiplier, MaxScaleMultiplier;
    [HideInInspector] public Vector3 minScale, maxScale;
    public float ScaleSpeed = 1;
    protected float scaleSpeed;
    public float AutoDecreaseSpeed;

    protected Vector3 newScale;

    public bool UpdateMass;
    protected Rigidbody rigid;
    public float MinMassMultiplier, MaxMassMultiplier;
    protected float newMass;
    protected float minMass, maxMass;


    protected float currentScaleAmount;

    public FixedJoint joint;
    public float minJointMassMultiplier, maxJointMassMultiplier;
    protected float minJointMass, maxJointMass;

    public bool pulleySystem;
    public ConfigurableJoint configurableJoint;
    public float linearMinLimit, linerMaxLimit;
    protected float currentLimit;

    protected float initScaleAmount;


    public override void SetInit(float scale)
    {
        newScale = Vector3.one * scale;
        if (newScale.x > maxScale.x)
        {
            newScale = maxScale;
        }
        else if (newScale.x < minScale.y)
        {
            newScale = minScale;
        }

        transform.localScale = newScale;
        if (UpdateMass)
        {
            rigid.mass = Mathf.Lerp(minMass, maxMass,
                GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
            if (joint)
            {
                joint.massScale = Mathf.Lerp(maxJointMass, minJointMass,
                    GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
            }
        }

        if (pulleySystem)
        {
            currentLimit = Mathf.Lerp(linearMinLimit, linerMaxLimit,
                GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
            SoftJointLimit temp = new SoftJointLimit {limit = currentLimit};
            configurableJoint.linearLimit = temp;
        }
    }

    protected virtual IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(.5f);
        transform.localScale = maxScale;
        if (UpdateMass)
        {
            rigid.mass = maxMass;
            if (joint)
                joint.massScale = maxJointMass;
            
        }
        if (pulleySystem)
        {
            SoftJointLimit temp = new SoftJointLimit{limit = linerMaxLimit};
            configurableJoint.linearLimit = temp;
        }
    }
    
    public override void SetMax()
    {
        StartCoroutine(WaitTime());
    }

    protected virtual void Start()
    {
        minScale = transform.localScale * MinScaleMultiplier;
        maxScale = transform.localScale * MaxScaleMultiplier;
        if (UpdateMass)
        {
            rigid = GetComponent<Rigidbody>();
            if (!rigid)
            {
                rigid = gameObject.AddComponent<Rigidbody>();
            }

            minMass = rigid.mass * MinMassMultiplier;
            maxMass = rigid.mass * MaxMassMultiplier;
            if (joint)
            {

                minJointMass = joint.massScale * minJointMassMultiplier;
                maxJointMass = joint.massScale * maxJointMassMultiplier;
            }

            if (pulleySystem)
            {
                initScaleAmount = transform.localScale.x;
                currentLimit = linearMinLimit;
                SoftJointLimit temp = new SoftJointLimit {limit = linearMinLimit};
                configurableJoint.linearLimit = temp;
            }
        }
    }

    public override bool ScaleUp(bool deltaTimed)
    {
        bool scaled = false;
        scaleSpeed = (deltaTimed) ? ScaleSpeed * Time.deltaTime : ScaleSpeed;
        if (transform.localScale.x < maxScale.x)
        {
            scaled = true;
            newScale = transform.localScale;
            newScale += scaleSpeed * Vector3.one;
            if (newScale.x > maxScale.x)
            {
                newScale = maxScale;
                scaled = false;
            }
            transform.localScale = newScale;
            if (UpdateMass)
            {
                rigid.mass = Mathf.Lerp(minMass, maxMass,
                    GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
                if (joint)
                {
                    joint.massScale = Mathf.Lerp(maxJointMass, minJointMass,
                        GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
                }
            }

            if (pulleySystem)
            {
                currentLimit = Mathf.Lerp(linearMinLimit, linerMaxLimit,
                    GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
                SoftJointLimit temp = new SoftJointLimit{limit = currentLimit};
                configurableJoint.linearLimit = temp;
            }
        }
        return scaled;
    }

    public override bool ScaleDown(bool deltaTimed)
    {
        bool scaled = false;
        scaleSpeed = (deltaTimed) ? ScaleSpeed * Time.deltaTime : ScaleSpeed;
        if (transform.localScale.x > minScale.x)
        {
            scaled = true;
            newScale = transform.localScale;
            newScale -= scaleSpeed * Vector3.one;
            if (newScale.x < minScale.x)
            {
                scaled = false;
                newScale = minScale;
            }
            transform.localScale = newScale;
            if (UpdateMass)
            {
                rigid.mass = Mathf.Lerp(minMass, maxMass,
                    GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
                if (joint)
                {
                    joint.massScale = Mathf.Lerp(maxJointMass, minJointMass,
                        GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
                }
            }
            if (pulleySystem)
            {
                currentLimit = Mathf.Lerp(linearMinLimit, linerMaxLimit,
                    GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
                SoftJointLimit temp = new SoftJointLimit{limit = currentLimit};
                configurableJoint.linearLimit = temp;
            }
        }
        return scaled;
    }

    public override void AutoScale(float time, bool Up)
    {
        StartCoroutine(ScaleAuto(time, Up));
    }

    private IEnumerator ScaleAuto(float time, bool Up)
    {
        float currentTime = 0;
        while(currentTime < time)
        {
            currentTime += Time.deltaTime;
            if (Up)
            {
                if (transform.localScale.x < maxScale.x)
                {
                    newScale = transform.localScale;
                    newScale += AutoDecreaseSpeed * Time.deltaTime * Vector3.one;
                    if (newScale.x > maxScale.x)
                    {
                        newScale = maxScale;
                    }
                    transform.localScale = newScale;
                    if (UpdateMass)
                    {
                        rigid.mass = Mathf.Lerp(minMass, maxMass,
                            GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
                        if (joint)
                        {
                            joint.massScale = Mathf.Lerp(maxJointMass, minJointMass,
                                GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
                        }
                    }

                    if (pulleySystem)
                    {
                        currentLimit = Mathf.Lerp(linearMinLimit, linerMaxLimit,
                            GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
                        SoftJointLimit temp = new SoftJointLimit { limit = currentLimit };
                        configurableJoint.linearLimit = temp;
                    }
                }
            }
            else
            {
                if (transform.localScale.x > minScale.x)
                {
                    newScale = transform.localScale;
                    newScale -= AutoDecreaseSpeed * Time.deltaTime * Vector3.one;
                    if (newScale.x < minScale.x)
                    {
                        newScale = minScale;
                    }
                    transform.localScale = newScale;
                    if (UpdateMass)
                    {
                        rigid.mass = Mathf.Lerp(minMass, maxMass,
                            GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
                        if (joint)
                        {
                            joint.massScale = Mathf.Lerp(maxJointMass, minJointMass,
                                GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
                        }
                    }
                    if (pulleySystem)
                    {
                        currentLimit = Mathf.Lerp(linearMinLimit, linerMaxLimit,
                            GeneralFunctions.ConvertRange(minScale.x, maxScale.x, 0, 1, newScale.x));
                        SoftJointLimit temp = new SoftJointLimit { limit = currentLimit };
                        configurableJoint.linearLimit = temp;
                    }
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

}
