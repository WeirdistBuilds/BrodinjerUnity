using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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



    public BowString bowstring;

    private void Start()
    {
        running = false;
        aiming = false;
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
            
            //rotDirection = initRotation;
            //rotDirection.y = transform.rotation.eulerAngles.y;
            //transform.rotation = Quaternion.Euler(rotDirection);
            if(numArrows.value > 0)
                yield return _waitforbutton;
            if (!frozen && numArrows.value > 0)
            {
                CenterCursor.SetActive(true);
                cameraRotation.AnimationOffset = 0;
                bowstring.Pull();
                anim.SetTrigger("Bow Pull");
                BowPulled.Invoke();
                inUse = true;
                if (currWeapon)
                {
                    if (playermove.rotate != bowRotate)
                    {
                        playermove.SwapMovement(bowRotate, bowTranslate, playermove.extraControls);    
                    }
                    cameraRotation.StartTimeSwap(CameraSwapTime, thirdPersonCamera, bowCamera);   
                    StartTimeSwap(CameraSwapTime);
                    
                    currentTime = 0;
                    yield return new WaitForSeconds(reloadTime);
                    if (!Input.GetButton(useButton))
                    {
                        bowstring.Release();
                        anim.SetTrigger("Bow Released");
                        continue;
                    }
                    
                    currPower = 0;
                    currArrow = Instantiate(ArrowPrefab, ArrowPrefab.transform.parent);
                    currArrow.SetActive(true);
                    ArrowRB = currArrow.GetComponent<Rigidbody>();
                    while (Input.GetButton(useButton))
                    {
                        if (playermove.rotate != bowRotate)
                        {
                            playermove.SwapMovement(bowRotate, bowTranslate, playermove.extraControls);    
                        }
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
                    while (frozen)
                    {
                        yield return new WaitForFixedUpdate();
                    }

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
                    anim.SetTrigger("Bow Released");
                    playermove.SwapMovement(playermove.rotate, originalTranslate, playermove.extraControls);


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
            //playermove.SwapMovement(originalRotate, playermove.translate);
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
    
}
