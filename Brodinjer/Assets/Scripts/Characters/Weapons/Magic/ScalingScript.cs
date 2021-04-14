using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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
    public bool InitialScale;
    
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
    ScalingMagic temp;
    private bool running;
    public SoundController MagicChargeSound, MagicCastSound;
    public CharacterController _cc;
    public ParticleSystem MagicGood01, MagicGood02, MagicFail;

    public void SetInitial(bool value)
    {
        InitialScale = value;
    }

    public void BreakScalingConnection()
    {
        if(currSpell != null)
        {
            if(temp != null)
            {
                if(temp.scaleObj != null)
                {
                    temp.scaleObj.highlightFX.gameObject.SetActive(false);
                }
            }
            MagicGood01.Stop();
            MagicGood02.Stop();
            Destroy(currSpell);
                       
        }
        MagicInUse.value = false;
        aiming = false;

    }
    public override void Initialize()
    {
        currWeapon = true;
        MagicObj.SetActive(true);
        finalScale = MagicPrefab.transform.localScale;
        _waitforbutton = new WaitUntil(CheckInput);
        attack = Attack();
        MagicInUse.value = false;
        if (!running)
        {
            running = true;
            weaponFunc = StartCoroutine(Attack());
        }
        aiming = false;
    }

    public override IEnumerator Attack()
    {
        while (currWeapon)
        {
            if (!MagicInUse.value && !frozen && _cc.isGrounded)
            {
                if (MagicAmount.value > minMagicAmount)
                    yield return _waitforbutton;
                yield return new WaitForSeconds(.1f);
                if (!frozen && MagicAmount.value > minMagicAmount && _cc.isGrounded && Input.GetButton(useButton))
                {
                    CenterCursor.SetActive(true);
                    if (currWeapon)
                    {
                        inUse = true;
                        MagicInUse.value = true;
                        aiming = true;
                    }

                    if (currWeapon && MagicAmount.value > 0 && !frozen && _cc.isGrounded && Input.GetButton(useButton))
                    {
                        if (cameraRotation.cameraRotation != bowCamera)
                        {
                            if (freezeWhenAim)
                                playermove.SwapMovement(bowRotate, freezePlayer, playermove.extraControls);
                            else
                                playermove.SwapMovement(bowRotate, playermove.translate, playermove.extraControls);
                            cameraRotation.SwapRotation(bowCamera);
                            StartTimeSwap(CameraSwapTime);
                        }
                        anim.SetBool("Magic Equipped", true);
                        cameraRotation.StartTimeSwap(CameraSwapTime, thirdPersonCamera, bowCamera);
                        StartTimeSwap(CameraSwapTime);

                        currPower = 0;
                        currSpell = Instantiate(MagicPrefab, InitPos);
                        currSpell.transform.localScale = Vector3.zero;
                        currSpell.SetActive(true);
                        SpellBall = currSpell.GetComponentInChildren<Rigidbody>();
                        MagicChargeSound.Play();
                        while (Input.GetButton(useButton) && MagicAmount.value > 0 && !frozen)
                        {
                            if (cameraRotation.cameraRotation != bowCamera)
                            {
                                if (freezeWhenAim)
                                    playermove.SwapMovement(bowRotate, freezePlayer, playermove.extraControls);
                                else
                                    playermove.SwapMovement(bowRotate, playermove.translate, playermove.extraControls);
                                StartTimeSwap(CameraSwapTime);
                            }

                            //cameraRotation.StartTimeSwap(CameraSwapTime, thirdPersonCamera, bowCamera);
                            if (currPower >= MaxPower)
                            {
                                currPower = MaxPower;
                            }
                            else if(currSpell != null)
                            {
                                currPower += Time.deltaTime * PowerIncreaseScale;
                                currSpell.transform.localScale = Vector3.Lerp(Vector3.zero, finalScale, GeneralFunctions.ConvertRange(0, MaxPower, 0, 1, currPower));
                                MagicAmount.SubFloat(decreaseSpeed * Time.deltaTime);
                            }

                            yield return _fixedUpdate;
                        }
                        while (!_cc.isGrounded)
                        {
                            Debug.Log("Not Grounded");
                            yield return new WaitForFixedUpdate();
                        }
                        if (!frozen)
                        {
                            anim.SetTrigger(AttackTrigger);
                            yield return new WaitForSeconds(.25f);
                            aiming = false;
                            CenterCursor.SetActive(false);
                            if (SpellBall != null)
                                SpellBall.constraints = RigidbodyConstraints.FreezeRotation;
                            if (currSpell != null)
                            {
                                currSpell.transform.parent = null;
                                temp = currSpell.GetComponentInChildren<ScalingMagic>();
                            }
                            if (temp && temp.VFX)
                                temp.VFX.SetActive(true);
                            temp.Fire();
                            MagicCastSound.Play();
                            if (SpellBall != null)
                                SpellBall.AddForce(Direction.transform.forward * currPower, ForceMode.Impulse);
                            currentSpellDuration = maxSpellDuration * (currPower / MaxPower);
                        }
                        else
                        {
                            if(SpellBall != null)
                            {
                                Destroy(SpellBall.gameObject);
                            }
                        }
                        cameraRotation.SwapRotation(thirdPersonCamera);
                        yield return new WaitForSeconds(.1f);
                        playermove.SwapMovement(originalRotate, originalTranslate, playermove.extraControls);

                        anim.SetBool("Magic Equipped", false);
                        while (inUse && currentSpellDuration > 0 && MagicInUse.value)
                        {
                            currentSpellDuration -= .1f;
                            yield return new WaitForSeconds(.1f);
                        }

                        if (currSpell == null || temp == null || !temp.hitObj)
                        {
                            inUse = false;
                            MagicInUse.value = false;
                            MagicGood01.Stop();
                            MagicGood02.Stop();
                            MagicFail.gameObject.SetActive(true);
                            //Destroy(currSpell);
                            StartCoroutine(WaitDestroy());
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

    private IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(.1f);
        Destroy(currSpell.gameObject);
    }
    

    public override void End()
    {
        //and end stuff needed
        MagicObj.SetActive(false);
        inUse = false;
        currWeapon = false;
        running = false;
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

    public override void Off()
    {
        inUse = false;
        running = false;
        if (weaponFunc != null)
        {
            StopCoroutine(weaponFunc);
        }
    }

    public override void On()
    {
        Initialize();
    }

    public override void Activate()
    {
        MagicObj.SetActive(true);
    }
}
