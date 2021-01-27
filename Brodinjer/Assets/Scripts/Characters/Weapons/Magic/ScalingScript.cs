using System.Collections;
using UnityEngine;

public class ScalingScript : WeaponBase
{
    private WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
    private WaitUntil _waitforbutton;
    public GameObject MagicPrefab;
    private Rigidbody SpellBall;
    private float currPower;
    public float MaxPower, PowerIncreaseScale, ScaleIncreaseAmount;
    private GameObject currSpell;
    private Vector3 finalScale;
    public LimitFloatData MagicAmount;
    public GameObject MagicObj;
    public CameraRotationManager cameraRotation;
    public CameraRotationBase bowCamera;
    public CameraRotationBase thirdPersonCamera;
    public PlayerMovement playermove;
    public CharacterRotate bowRotate;
    public CharacterRotate originalRotate;
    private float currentSpellDuration;
    public bool freezeWhenAim;
    public CharacterTranslate freezePlayer;
    public CharacterTranslate originalTranslate;
    private Coroutine swapFunc;
    public float CameraSwapTime;
    public GameObject CenterCursor;
    
    //Magic Variables
    public Transform InitPos;
    //private Vector3 initRotation;
    public BoolData MagicInUse;
    public float decreaseSpeed;
    public float maxSpellDuration;
    private float currentTime;
    private bool aiming;
    public float minMagicAmount;
    public Transform Direction;


    public override void Initialize()
    {
        if (!currWeapon)
        {
            currWeapon = true;
            MagicObj.SetActive(true);
            finalScale = MagicPrefab.transform.localScale;
            _waitforbutton = new WaitUntil(CheckInput);
            attack = Attack();
            MagicInUse.value = false;
            weaponFunc = StartCoroutine(Attack());
            aiming = false;
        }

    }

    public override IEnumerator Attack()
    {
        Debug.Log("Start Attack");
        anim.SetBool("Magic Equipped", true);
        while (currWeapon)
        {
            if (!MagicInUse.value)
            {
                while (frozen)
                {
                    yield return new WaitForFixedUpdate();
                }

                if(MagicAmount.value > minMagicAmount)
                    yield return _waitforbutton;
                if (!frozen && MagicAmount.value > minMagicAmount)
                {
                    CenterCursor.SetActive(true);
                    if (currWeapon)
                    {
                        inUse = true;
                        MagicInUse.value = true;
                        aiming = true;
                    }

                    if (currWeapon && MagicAmount.value > 0 && !frozen)
                    {
                        if (cameraRotation.cameraRotation != bowCamera)
                        {
                            if(freezeWhenAim)
                                playermove.SwapMovement(bowRotate, freezePlayer, playermove.extraControls);
                            else
                                playermove.SwapMovement(bowRotate, playermove.translate, playermove.extraControls);
                            StartTimeSwap(CameraSwapTime);
                        }
                        cameraRotation.StartTimeSwap(CameraSwapTime, thirdPersonCamera, bowCamera);
                        StartTimeSwap(CameraSwapTime);
                        
                        currPower = 0;
                        currSpell = Instantiate(MagicPrefab, InitPos);
                        currSpell.transform.localScale = Vector3.zero;
                        currSpell.SetActive(true);
                        SpellBall = currSpell.GetComponentInChildren<Rigidbody>();
                        while (Input.GetButton(useButton) && MagicAmount.value > 0)
                        {
                            if (cameraRotation.cameraRotation != bowCamera)
                            {
                                if(freezeWhenAim)
                                    playermove.SwapMovement(bowRotate, freezePlayer, playermove.extraControls);
                                else
                                    playermove.SwapMovement(bowRotate, playermove.translate, playermove.extraControls);
                                StartTimeSwap(CameraSwapTime);
                            }

                            cameraRotation.StartTimeSwap(CameraSwapTime, thirdPersonCamera, bowCamera);
                            while (frozen)
                            {
                                yield return new WaitForFixedUpdate();
                            }

                            if (currPower >= MaxPower)
                            {
                                currPower = MaxPower;
                            }
                            else
                            {
                                currPower += Time.deltaTime * PowerIncreaseScale;
                                currSpell.transform.localScale = Vector3.Lerp(Vector3.zero, finalScale, GeneralFunctions.ConvertRange(0, MaxPower, 0, 1, currPower));
                                MagicAmount.SubFloat(decreaseSpeed * Time.deltaTime);
                            }

                            yield return _fixedUpdate;
                        }

                        while (frozen)
                        {
                            yield return new WaitForFixedUpdate();
                        }

                        aiming = false;
                        CenterCursor.SetActive(false);

                        SpellBall.constraints = RigidbodyConstraints.FreezeRotation;
                        currSpell.transform.parent = null;
                        ScalingMagic temp = currSpell.GetComponentInChildren<ScalingMagic>();
                        if(temp && temp.VFX)
                            temp.VFX.SetActive(true);
                        temp.Fire();
                        SpellBall.AddForce(Direction.transform.forward * currPower, ForceMode.Impulse);
                        currentSpellDuration = maxSpellDuration * (currPower / MaxPower);
                        playermove.SwapMovement(bowRotate, originalTranslate, playermove.extraControls);

                        while (inUse && currentSpellDuration > 0 && MagicInUse.value)
                        {
                            currentSpellDuration -= .1f;
                            yield return new WaitForSeconds(.1f);
                        }

                        if (currSpell == null || temp == null || !temp.hitObj)
                        {
                            inUse = false;
                            MagicInUse.value = false;
                            Destroy(currSpell);
                            try
                            {
                                ScalableObjectBase obj = temp.scaleObj;
                                if (obj != null)
                                {
                                    obj.highlightFX.UnHighlight();
                                }
                            }
                            catch
                            {
                                
                            }
                        }

                        inUse = false;
                    }
                }
            }

            yield return _fixedUpdate;

        }
    }
    

    public override void End()
    {
        //and end stuff needed
        MagicObj.SetActive(false);
        inUse = false;
        currWeapon = false;
        if (cameraRotation.cameraRotation != thirdPersonCamera)
        {
            cameraRotation.StopTimeSwap(thirdPersonCamera);
            playermove.SwapMovement(originalRotate, originalTranslate, playermove.extraControls);
        }

        if (weaponFunc != null)
        {
            StopCoroutine(weaponFunc);
        }
        anim.SetBool("Magic Equipped", false);
    }

    public void SpellHit(bool hit)
    {
        currentSpellDuration = 0;
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
            if(!aiming)
                currentTime -= .1f;
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
