using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BowandArrow : WeaponBase
{
    private WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
    private WaitUntil _waitforbutton;
    public GameObject ArrowPrefab;
    private Rigidbody ArrowRB;
    private float currPower, currentTime;
    public float MaxPower, PowerIncreaseScale;
    private GameObject currArrow;
    public GameObject WeaponObj;
    public CameraRotationManager cameraRotation;
    public CameraRotationBase bowCamera;
    public CameraRotationBase thirdPersonCamera;
    public PlayerMovement playermove;
    public CharacterRotate bowRotate;
    public CharacterRotate originalRotate;
    public CharacterTranslate bowTranslate;
    public CharacterTranslate originalTranslate;
    public LimitIntData numArrows;
    public float CameraSwapTime;
    private Coroutine swapFunc;
    public GameObject CenterCursor;
    
    //Arrow Variables
    public UnityEvent BowEquiped, BowPulled, ArrowFired, BowUnequipped;
    public float cooldowntime, reloadTime;
    private bool running, aiming, exited;
    private ResetTriggers reset;
    public SoundController DrawSound, FireSound;
    public CharacterController _cc;


    public BowString bowstring;

    private void Start()
    {
        running = false;
        aiming = false;
        reset = anim.GetComponent<ResetTriggers>();

    }

    private void FixedUpdate()
    {
        if (currWeapon)
        {
            if (!anim.GetBool("Pulled"))
            {
                bowstring.Release();
                inUse = false;
            }
        }
    }

    public override void Initialize()
    {
        BowEquiped.Invoke();
        _waitforbutton = new WaitUntil(CheckInput);
        currWeapon = true;
        WeaponObj.SetActive(true);
        attack = Attack();
        //initRotation = transform.rotation.eulerAngles;
        if (!running)
        {
            running = true;
            weaponFunc = StartCoroutine(Attack());
        }
    }

    public override IEnumerator Attack()
    {
        anim.SetBool("Bow Equipped", true);
        while (currWeapon)
        {
            while (frozen)
            {
                yield return new WaitForFixedUpdate();
            }
            
            if(numArrows.value > 0)
                yield return _waitforbutton;
            if (!frozen && numArrows.value > 0 && _cc.isGrounded)
            {
                CenterCursor.SetActive(true);
                cameraRotation.AnimationOffset = 0;
                bowstring.Pull();
                reset.ResetAllTriggers();
                anim.SetBool("Pulled", true);
                BowPulled.Invoke();
                inUse = true;
                if (currWeapon)
                {
                    if (playermove.translate != bowTranslate)
                    {
                        playermove.SwapMovement(bowRotate, bowTranslate, playermove.extraControls);
                    }
                    cameraRotation.StartTimeSwap(CameraSwapTime, thirdPersonCamera, bowCamera);
                    StartTimeSwap(CameraSwapTime);

                    //currentTime = 0;

                    yield return new WaitForSeconds(reloadTime);
                    DrawSound.Play();


                    if (!Input.GetButton(useButton))
                    {
                        reset.ResetAllTriggers();
                        anim.SetBool("Pulled", false);
                        if (currArrow != null)
                            Destroy(currArrow);
                        bowstring.Release();
                        playermove.SwapMovement(playermove.rotate, originalTranslate, playermove.extraControls);
                        continue;
                    }
                    else
                    {

                        currPower = 0;
                        currArrow = Instantiate(ArrowPrefab, ArrowPrefab.transform.parent);
                        currArrow.SetActive(true);
                        ArrowRB = currArrow.GetComponent<Rigidbody>();
                        yield return new WaitForSeconds(.5f);
                        if (!Input.GetButton(useButton))
                        {
                            bowstring.Release();
                            reset.ResetAllTriggers();
                            anim.SetBool("Pulled", false);
                            if(currArrow!= null)
                                Destroy(currArrow);
                            playermove.SwapMovement(playermove.rotate, originalTranslate, playermove.extraControls);
                            continue;
                        }
                        if (playermove.rotate != bowRotate)
                        {
                            playermove.SwapMovement(bowRotate, bowTranslate, playermove.extraControls);
                        }
                        while (Input.GetButton(useButton))
                        {
                            cameraRotation.StartTimeSwap(CameraSwapTime, thirdPersonCamera, bowCamera);
                            StartTimeSwap(CameraSwapTime);
                            aiming = true;

                            while (frozen)
                            {
                                yield return new WaitForFixedUpdate();
                            }
                            yield return _fixedUpdate;
                        }
                        currPower = MaxPower;
                        while (!_cc.isGrounded || frozen)
                        {
                            yield return new WaitForFixedUpdate();
                        }
                        FireSound.Play();
                        bowstring.Release();
                        cameraRotation.AnimationOffset = -2;
                        numArrows.SubInt(1);
                        ArrowFired.Invoke();
                        ArrowRB.constraints = RigidbodyConstraints.FreezeRotation;
                        currArrow.transform.parent = null;
                        ArrowRB.AddForce(currArrow.GetComponent<Arrow>().Direction.forward * currPower, ForceMode.Impulse);
                        currArrow.GetComponent<Arrow>().Fired();
                        inUse = false;
                        aiming = false;
                        CenterCursor.SetActive(false);
                        cameraRotation.PauseTime(false);
                        yield return new WaitForSeconds(cooldowntime);
                        currArrow = null;
                        anim.SetBool("Pulled", false);
                        bowstring.Release();
                        playermove.SwapMovement(playermove.rotate, originalTranslate, playermove.extraControls);
                    }
                }
    
            }
            yield return new WaitForFixedUpdate();
        }

        running = false;

    }

    public override void End()
    {
        //and end stuff needed
        running = false;
        aiming = false;
        cameraRotation.PauseTime(false);
        BowUnequipped.Invoke();
        WeaponObj.SetActive(false);
        currWeapon = false;
        inUse = false;
        if (cameraRotation.cameraRotation != thirdPersonCamera)
        {
            cameraRotation.StopTimeSwap(thirdPersonCamera);
            StopTimeSwap();
            playermove.SwapMovement(originalRotate, originalTranslate);
        }
        if(weaponFunc != null)
            StopCoroutine(weaponFunc);
        anim.SetBool("Bow Equipped", false);
    }

    private bool CheckInput()
    {
        if (Input.GetButtonDown(useButton) || !currWeapon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetBowPulledEvent(Unity_Event_Holder e)
    {
        if(e)
            BowPulled = e.Event;
        else
            BowPulled = new UnityEvent();
    }

    public void SetArrowFiredEvent(Unity_Event_Holder e)
    {
        if(e)
            ArrowFired = e.Event;
        else
            ArrowFired = new UnityEvent();
        
    }

    public void SetBowUnequipped(Unity_Event_Holder e)
    {
        if(e)
            BowUnequipped = e.Event;
        else
            BowUnequipped = new UnityEvent();
        
    }

    public void StartTimeSwap(float time)
    {
        if (swapFunc == null)
        {
            currentTime = time;
            swapFunc = StartCoroutine(TimedSwap());
        }
        else
        {
            currentTime = time;
        }
        
    }

    private IEnumerator TimedSwap()
    {
        while (currentTime > 0)
        {
            if (!aiming)
            {
                currentTime -= .1f;
            }
            yield return new WaitForSeconds(.1f);
        }
        StopTimeSwap();
    }

    public void StopTimeSwap()
    {
        if (swapFunc != null)
        {
            playermove.SwapMovement(originalRotate, originalTranslate, playermove.extraControls);
            StopCoroutine(swapFunc);
        }
        else if(originalRotate != playermove.rotate)
        {
            playermove.SwapMovement(originalRotate, originalTranslate, playermove.extraControls);
        }
        swapFunc = null;

    }

    public override void Off()
    {
        running = false;
        aiming = false;
        inUse = false;
        if (weaponFunc != null)
            StopCoroutine(weaponFunc);
    }

    public override void On()
    {
        Initialize();
    }

    public override void Activate()
    {
        WeaponObj.SetActive(true);
    }
}
