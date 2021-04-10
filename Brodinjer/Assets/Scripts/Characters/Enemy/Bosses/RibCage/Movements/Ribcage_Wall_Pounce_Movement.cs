using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ribcage_Wall_Pounce_Movement : Enemy_Attack_Base
{
    [Header("Jump Variables")]
    public float gravity = 10;
    public float jumpRange = 10;
    public float WallJumpTime = .5f;
    public float ForwardJumpForce, UpwardJumpForce;
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
    public float WallPounceForce;
    public float WallPounceInitTime;
    
    
    private bool checkJump, Up, calculateGravity;
    private Rigidbody RB;
    private BoxCollider col;
    private Vector3 surfaceNormal, myNormal, jumpDirection, moveDirection, pounceDirection, rot, myForward, direction, orgPos, dstPos;
    private float distGround,  currentTime = 0, randomTimeChange, currentTimeChange,
        randomWallRotationSpeed, randomTimeWait, currentTimeWait, randomWallSpeed, currentWallSpeed,
        randomWallCrawlTime, currentWallCrawlTime;    
    private Ray ray;
    private RaycastHit hit;
    private Quaternion targetRot, orgRot, dstRot;
    private Coroutine moveFunc, jumpFunc, gravityFunc;
    private bool isGrounded;
    private float deltaGround = .2f;

    public override void Init()
    {
        base.Init();
        RB = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
    }

    public override IEnumerator Attack()
    {
        Debug.Log("Wall Attack");
        StartCoroutine(GravityForce());
        checkJump = true;
        StartCoroutine(CheckJump(WallJumpTime));
        distGround = col.bounds.extents.y - col.center.y;
        RB.freezeRotation = true;
        myNormal = enemyObj.transform.up;

        yield return new WaitForSeconds(AttackStartTime);
        jumpDirection = enemyObj.transform.forward * ForwardJumpForce + enemyObj.transform.up * UpwardJumpForce;
        RB.AddForce(jumpDirection, ForceMode.Impulse);
        yield return new WaitForSeconds(jumpAfterTime);
        checkJump = false;
        currentTime = 90 / InitRotateSpeed;
        while (currentTime > 0)
        {
            enemyObj.transform.Rotate(0, InitRotateSpeed * Time.deltaTime, 0);
            currentTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        Vector3 rot = enemyObj.transform.rotation.eulerAngles;
        rot.x = 0;
        enemyObj.transform.rotation = Quaternion.Euler(rot);
        yield return new WaitForFixedUpdate();

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

        while (currentWallCrawlTime < randomWallCrawlTime)
        {
            currentWallCrawlTime += Time.deltaTime;
            if (currentWallSpeed < randomWallSpeed)
            {
                currentWallSpeed += Time.deltaTime * WallAcceleration;
            }
            else if (currentWallSpeed > randomWallSpeed)
            {
                currentWallSpeed -= Time.deltaTime * WallAcceleration;
            }

            ray = new Ray(enemyObj.transform.position, -myNormal); // cast ray downwards
            if (Physics.Raycast(ray, out hit))
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

            myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
            Vector3 myForward = Vector3.Cross(enemyObj.transform.right, myNormal);
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

            enemyObj.transform.rotation = Quaternion.Lerp(enemyObj.transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
            enemyObj.transform.Translate(0, 0, Time.deltaTime * currentWallSpeed);
            yield return new WaitForFixedUpdate();
        }

        currentTime = ((90 + enemyObj.transform.rotation.eulerAngles.x)%360) / InitRotateSpeed;
        while (currentTime > 0)
        {
            enemyObj.transform.Rotate(0, InitRotateSpeed * Time.deltaTime, 0);
            currentTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        
        yield return new WaitForSeconds(WallPounceInitTime);

        pounceDirection = (player.transform.position - enemyObj.transform.position).normalized; 
        yield return new WaitForSeconds(.1f);
        RB.AddForce(pounceDirection*WallPounceForce, ForceMode.Impulse);
        yield return new WaitForSeconds(.25f);
        checkJump = true;
        StartCoroutine(CheckJump(WallJumpTime));
        yield return new WaitForSeconds(CoolDownTime);
        checkJump = false;

    }

    private IEnumerator GravityForce()
    {
        while (true)
        {
            RB.AddForce(-gravity*RB.mass*myNormal);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator CheckJump(float jumpTime)
    {
        Debug.Log("Start CheckJump");
        while (checkJump)
        {
            Debug.Log("Check Jump");
            if (Physics.Raycast(enemyObj.transform.position, enemyObj.transform.forward, out hit, jumpRange))
            {
                // wall ahead?
                StartCoroutine(RotateToWall(hit.point, hit.normal, jumpTime)); // yes: jump to the wall
            }
            if (Physics.Raycast(enemyObj.transform.position, -myNormal, out hit)){ // use it to update myNormal and isGrounded
                surfaceNormal = hit.normal;
            }
            else {
                surfaceNormal = Vector3.up; 
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator RotateToWall(Vector3 point, Vector3 normal, float jumpTime)
    {
        RB.isKinematic = true;
        Vector3 orgPos = enemyObj.transform.position;
        Quaternion orgRot = enemyObj.transform.rotation;
        Vector3 dstPos = point + (normal * (distGround + 0.5f));
        Vector3 myForward = Vector3.Cross(enemyObj.transform.right, normal);
        Quaternion dstRot = Quaternion.LookRotation(myForward, normal);
        float t = 0f;
        while (t < jumpTime)
        {
            t += Time.deltaTime;
            enemyObj.transform.position = Vector3.Lerp(orgPos, dstPos, GeneralFunctions.ConvertRange(0,jumpTime, 0, 1, t));
            enemyObj.transform.rotation = Quaternion.Slerp(orgRot, dstRot, GeneralFunctions.ConvertRange(0,jumpTime, 0, 1, t));
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

}
