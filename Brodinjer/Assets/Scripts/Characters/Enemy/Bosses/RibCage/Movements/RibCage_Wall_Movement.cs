using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RibCage_Wall_Movement : Enemy_Attack_Base
{
    public Transform FrontObj;
    public Transform PlayerObj;
    public GameObject WeaponObj;
    
    [Header("Jump Variables")]
    public float gravity = 10;
    public float jumpRange = 10;
    public float WallJumpTime = .5f;
    public float jumpInitTime;
    public float ForwardJumpForce, UpwardJumpForce;
    public float deltaGround = .2f;
    public float lerpSpeed = 10f;
    public float jumpAfterTime;

    [Header("Wall Move Variables")]
    public float InitRotateSpeed;
    public float minTimeChange, maxTimeChange;
    public float minWallRotationSpeed, maxWallRotationSpeed;
    public float minTimeWait, maxTimeWait;
    public float minWallSpeed, maxWallSpeed;
    public float WallAcceleration;
    
    [Header("Wall Pounce Variables")]
    public float MinWallCrawlTime;
    public float MaxWallCrawlTime;
    public float WallForwardForce;
    public float WallPounceInitTime;
    public float WallPounceEndTime;
    public float FinishTime;

    public UnityEvent InitEvent, FinishEvent;
    
    private bool checkJump, isGrounded, Up;
    private Rigidbody RB;
    private BoxCollider col;
    private Vector3 surfaceNormal, myNormal, jumpDirection, moveDirection, pounceDirection;
    private float distGround, currentTime = 0, randomTimeChange, currentTimeChange,
        randomWallRotationSpeed, randomTimeWait, currentTimeWait, randomWallSpeed, currentWallSpeed,
        randomWallCrawlTime, currentWallCrawlTime;    
    private Ray ray;
    private RaycastHit hit, walldestination;
    public float PouncePauseTime;

    public LayerMask wallLayer;
    private bool jumping;

    public SoundController walkSound;

    private bool walking;
    public float MaxFootstep, MinFootstep;

    private bool toGround;

    //public BoxCollider boxcollider;

    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        myNormal = transform.up;
        StartCoroutine(GravityForce());
        jumping = false;
        resetAnims = animator.GetComponent<ResetTriggers>();
    }

    private IEnumerator WallWalkSpeed()
    {
        while (walking)
        {
            if (currentWallSpeed >= .1f)
            {
                walkSound.Play();
                yield return new WaitForSeconds(GeneralFunctions.ConvertRange(0, maxWallSpeed,
                    MaxFootstep, MinFootstep, currentWallSpeed));
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public override void StartAttack()
    {
        RB.isKinematic = false;
        StartCoroutine(Attack());
    }

    public override IEnumerator Attack()
    {
        toGround = false;
        attacking = true;
        RB.useGravity = false;
        WeaponObj.SetActive(false);     
        myNormal = transform.up;
        RB.freezeRotation = true;
        distGround = col.bounds.extents.y - col.center.y;
        InitEvent.Invoke();
        if (resetAnims)
            resetAnims.ResetAllTriggers();
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(jumpInitTime);
        jumpDirection = transform.forward * ForwardJumpForce + transform.up * UpwardJumpForce;
        //Debug.DrawRay(transform.position, jumpDirection.normalized * 100, Color.red, 10);

        //RB.velocity = Vector3.zero;
        //RB.AddForce(jumpDirection, ForceMode.Impulse);
        transform.Rotate(-90, 0, 0);
        jumping = true;
        attackSound.Play();
        Physics.Raycast(transform.position, jumpDirection.normalized, out walldestination, 100, wallLayer);
        currentTime = 0;
        while(currentTime < jumpAfterTime)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, walldestination.point, 10 * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        //boxcollider.isTrigger = true;
        //yield return new WaitForSeconds(jumpAfterTime);
        yield return new WaitForSeconds(.5f);
        animator.SetFloat("Speed", 1);
        currentTime = 90 / InitRotateSpeed;
        while (currentTime > 0)
        {
            transform.Rotate(0, InitRotateSpeed * Time.deltaTime, 0);
            currentTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = 0;
        transform.rotation = Quaternion.Euler(rot);
        yield return new WaitForFixedUpdate();
        animator.SetTrigger("Land");
        jumping = false;

        randomTimeChange = Random.Range(minTimeChange, maxTimeChange);
        currentTimeChange = randomTimeChange;
        randomWallRotationSpeed = Random.Range(minWallRotationSpeed, maxWallRotationSpeed);
        randomTimeWait = Random.Range(minTimeWait, maxTimeWait);
        currentTimeWait = randomTimeWait;
        randomWallSpeed = Random.Range(minWallSpeed, maxWallSpeed);
        currentWallSpeed = 0;
        Up = false;
        randomWallCrawlTime = Random.Range(MinWallCrawlTime, MaxWallCrawlTime);
        currentWallCrawlTime = 0;
        walking = false;
        while (currentWallCrawlTime < randomWallCrawlTime)
        {
            if (!walking)
            {
                walking = true;
                StartCoroutine(WallWalkSpeed());
            }
            currentWallCrawlTime += Time.deltaTime;
            if (currentWallSpeed < randomWallSpeed)
            {
                currentWallSpeed += Time.deltaTime * WallAcceleration;
            }
            else if (currentWallSpeed > randomWallSpeed)
            {
                currentWallSpeed -= Time.deltaTime * WallAcceleration;
            }



            myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
            Vector3 myForward = Vector3.Cross(transform.right, myNormal);
            Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
            if (currentTimeChange > 0)
            {
                if (Up)
                {
                    Vector3 direction = targetRot.eulerAngles;
                    direction.x += randomWallRotationSpeed * Time.deltaTime;
                    targetRot = Quaternion.Euler(direction);
                }
                else
                {
                    Vector3 direction = targetRot.eulerAngles;
                    direction.x += -1 * randomWallRotationSpeed * Time.deltaTime;
                    targetRot = Quaternion.Euler(direction);

                }

                currentTimeChange -= Time.deltaTime;
            }
            else if (currentTimeWait > 0)
            {
                currentTimeWait -= Time.deltaTime;
            }
            else
            {
                float newRandomChange = Random.Range(minTimeChange, maxTimeChange);
                currentTimeChange = randomTimeChange + newRandomChange;
                randomTimeChange = newRandomChange;
                float newRandomWait = Random.Range(minTimeWait, maxTimeWait);
                currentTimeWait = randomTimeWait + newRandomWait;
                randomTimeWait = newRandomWait;
                Up = !Up;
                randomWallSpeed = Random.Range(minWallSpeed, maxWallSpeed);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
            transform.Translate(0, 0, Time.deltaTime * currentWallSpeed);
            yield return new WaitForFixedUpdate();
        }
        walking = false;
        currentTime = ((90 + transform.rotation.eulerAngles.x)%360) / InitRotateSpeed;
        while (currentTime > 0)
        {
            transform.Rotate(0, InitRotateSpeed * Time.deltaTime, 0);
            currentTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.rotation = Quaternion.Euler(new Vector3(-270,transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        animator.SetFloat("Speed", 0);
        yield return new WaitForSeconds(WallPounceInitTime);
        pounceDirection = (PlayerObj.transform.position - transform.position).normalized;
        if (resetAnims)
            resetAnims.ResetAllTriggers();
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(PouncePauseTime);
        attackSound.Play();
        WeaponObj.SetActive(true);
        //RB.AddForce(pounceDirection*WallForwardForce, ForceMode.Impulse);
        transform.Rotate(-90,0,0);
        Physics.Raycast(transform.position, pounceDirection.normalized, out walldestination, 100, wallLayer);
        currentTime = 0;
        while (currentTime < jumpAfterTime)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, walldestination.point, 10 * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        //boxcollider.isTrigger = true;
        jumping = true;
        yield return new WaitForSeconds(WallPounceEndTime);
        toGround = true;
        RB.freezeRotation = false;
        WeaponObj.SetActive(false);
        yield return new WaitForSeconds(FinishTime);
        FinishEvent.Invoke();
        attacking = false;
        RB.useGravity = false;
        RB.isKinematic = true;
        animator.SetTrigger("Land");
        jumping = false;
    }

    private IEnumerator GravityForce()
    {
        while (true)
        {
            ray = new Ray(transform.position, -myNormal); // cast ray downwards
            if (Physics.Raycast(ray, out hit, wallLayer))
            {
                // use it to update myNormal and isGrounded
                isGrounded = hit.distance <= distGround + deltaGround;
                surfaceNormal = hit.normal;
            }
            else
            {
                isGrounded = false;
                surfaceNormal = Vector3.up;
            }

            myNormal = transform.up;
            RB.AddForce(-gravity*RB.mass*myNormal, ForceMode.Force);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator RotateToWall(Vector3 point, Vector3 normal, float jumpTime)
    {
        RB.isKinematic = true;
        Vector3 orgPos = transform.position;
        Quaternion orgRot = transform.rotation;
        Vector3 dstPos = point + (normal * (distGround + 0.5f));
        Vector3 myForward = Vector3.Cross(transform.right, normal);
        Quaternion dstRot = Quaternion.LookRotation(myForward, normal);
        float t = 0f;
        while (t < jumpTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(orgPos, dstPos, GeneralFunctions.ConvertRange(0,jumpTime, 0, 1, t));
            transform.rotation = Quaternion.Slerp(orgRot, dstRot, GeneralFunctions.ConvertRange(0,jumpTime, 0, 1, t));
            yield return new WaitForFixedUpdate();
        }
        myNormal = normal;
        RB.isKinematic = false;
        checkJump = false;

    }
    
    public bool CheckDestination(Vector3 Dest01, Vector3 Dest02, float offset)
    {
        if ((Dest01.x >= Dest02.x - offset
             && Dest01.x <= Dest02.x + offset)
            &&(Dest01.y >= Dest02.y - offset
               && Dest01.y <= Dest02.y + offset)
            &&(Dest01.z >= Dest02.z - offset
               && Dest01.z <= Dest02.z + offset))
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            //boxcollider.isTrigger = false;

            jumping = false;
            animator.SetTrigger("Land");
            if (toGround)
            {
                RB.isKinematic = true;
                RB.useGravity = false;
                toGround = false;
            }
        }
    }

}
