using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ScalingMagic : MonoBehaviour
{
    //Add Local scale point
    public PlayerMovement movement;
    public WeaponManager wm;
    private Transform ScalingObj;
    public float ScaleTime;
    private WaitForFixedUpdate _fixedUpdate;
    [HideInInspector]
    public ScalableObjectBase scaleObj;
    public float IncreaseAmount;
    public string stopButton;
    [HideInInspector] public bool hitObj = false;
    public GameObject art;
    public LimitFloatData MagicAmount;
    public BoolData MagicInUse;
    public float decreaseSpeed;
    public ScalingScript scalescript;
    public GameObject VFX;
    public string ScaleAxis;
    public GameObject MagicCollider, VFXCollider;
    public GameObject InitialScaleTutorial, ScaleTutorial;
    public ParticleSystem MagicGood01, MagicGood02, MagicFail, MagicSuccess;

    private void Start()
    {
        hitObj = false;
        _fixedUpdate = new WaitForFixedUpdate();
    }
    
    public void Fire()
    {
        MagicGood01.Play();
        MagicGood02.Play();
        MagicCollider.SetActive(true);
        VFXCollider.SetActive(true); 
        //GetComponentInParent<Look_At_Script>().StopLookAt();
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (!hitObj && !scalescript.frozen)
        {
            //VFX.transform.parent = null;
            //VFX.GetComponent<Destroy_Object_Timed>().Call();
            if (other.CompareTag("Scalable"))
            {
                scalescript.SpellHit(true);
                hitObj = true;
                ScalingObj = other.gameObject.transform;
                scaleObj = ScalingObj.GetComponent<ScalableObjectBase>();
                if (scaleObj == null)
                    scaleObj = ScalingObj.GetComponentInParent<ScalableObjectBase>();
                art.SetActive(false);
                StartCoroutine(Scale());
            }
            else
            {
                yield return new WaitForSeconds(.05f);
                if (!hitObj)
                {
                    scalescript.SpellHit(false);
                    MagicInUse.value = false;
                    Destroy(this);
                    MagicFail.gameObject.SetActive(true);
                    MagicGood01.Stop();
                    MagicGood02.Stop();
                }
            }


        }

    }

    private IEnumerator Scale()
    {
        //movement.StopAll();
        if (scalescript.InitialScale)
        {
            scalescript.InitialScale = false;
            InitialScaleTutorial.SetActive(true);
        }
        else
        {
            ScaleTutorial.SetActive(true);
            MagicSuccess.gameObject.SetActive(true);
        }
        bool soundon = false;
        if (scaleObj != null)
        {
            scaleObj.highlightFX.Highlight();
            scalescript.inUse = true;
            while (MagicAmount.value > 0 && !scalescript.frozen && scalescript.currWeapon)
            {
                if (Input.GetAxis(ScaleAxis) > 0)
                {

                    MagicAmount.SubFloat(decreaseSpeed * Time.deltaTime);
                    if (scaleObj.ScaleUp(true))
                    {
                        if (!soundon)
                        {
                            soundon = true;
                            scaleObj.growSound.Play();
                        }
                    }
                    else
                    {
                        if (soundon)
                        {
                            soundon = false;
                            scaleObj.growSound.Stop();
                        }
                    }
                }
                else if (Input.GetAxis(ScaleAxis) < 0)
                {
                    MagicAmount.SubFloat(decreaseSpeed * Time.deltaTime);
                    if (scaleObj.ScaleDown(true))
                    {
                        if (!soundon)
                        {
                            soundon = true;
                            scaleObj.growSound.Play();
                        }
                    }
                    else
                    {
                        if (soundon)
                        {
                            soundon = false;
                            scaleObj.growSound.Stop();
                        }
                    }
                }
                else
                {
                    if (soundon)
                    {
                        soundon = false;
                        scaleObj.growSound.Stop();
                    }
                }
                if (Input.GetButtonDown(stopButton))
                {
                    Destroy(this.gameObject);

                }
                yield return _fixedUpdate;
            }
            if(MagicAmount.value <= 0)
            {
                MagicFail.gameObject.SetActive(true);
                MagicGood01.Stop();
                MagicGood02.Stop();
            }
        }
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        ScaleTutorial.SetActive(false);
        MagicInUse.value = false;
        MagicGood01.Stop();
        MagicGood02.Stop();
        if (scaleObj != null)
        {
            scaleObj.highlightFX.UnHighlight();
            scaleObj.growSound.Stop();
        }
    }
}
