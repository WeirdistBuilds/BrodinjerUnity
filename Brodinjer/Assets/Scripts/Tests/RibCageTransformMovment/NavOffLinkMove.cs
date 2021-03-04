using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class NavOffLinkMove : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform Destination;
    private RaycastHit hit;
    public LayerMask layer;
    public float HitDistance, RotateSpeed, TranslateAmount;
    private float currentTime, translateSpeed, rotateySpeed, yrotation, rotatezSpeed, zrotation, xrotation;
    private NavMeshSurface newsurface;
    private Vector3 newRotation, newTranslation;
    private bool climbing;
    private TransformPosition currentPos, newPos;
    public TransformPosition StartPosition;

    public enum TransformPosition
    {
        Floor,
        WallX,
        WallZ,
        Cieling
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPos = StartPosition;
        climbing = false;
        agent.destination = Destination.position;
    }

    private void Update()
    {
        agent.destination = Destination.position;
        if(Physics.Raycast(agent.transform.position, transform.forward, out hit, HitDistance))
        {
            if(!climbing)
                StartCoroutine(Call());
        }
    }

    private IEnumerator Call()
    {
        if (!climbing)
        {
            climbing = true;
            agent.updateRotation = false;
            agent.enabled = false;
            switch (currentPos)
            {
                case TransformPosition.Floor:
                    newPos = CheckTransformDirection();
                    currentTime = 90 / RotateSpeed;
                    translateSpeed = TranslateAmount / currentTime;
                    yrotation = agent.transform.rotation.eulerAngles.y % 360;

                    if (yrotation > 180)
                    {
                        yrotation -= 360;
                    }

                    rotateySpeed = yrotation / currentTime;
                    while (currentTime >= 0)
                    {
                        agent.transform.Rotate(-RotateSpeed * Time.deltaTime, 0, 0,
                            Space.Self);
                        agent.transform.Translate(0, 0, translateSpeed * Time.deltaTime, agent.transform);
                        currentTime -= Time.deltaTime;
                        yield return new WaitForFixedUpdate();
                    }

                    agent.transform.Rotate(0, 0, -yrotation, Space.Self);
                    currentPos = newPos;
                    break;
                case TransformPosition.WallX:
                    newPos = CheckTransformDirection();
                    switch (newPos)
                    {
                        case TransformPosition.WallZ:
                            currentTime = 90 / RotateSpeed;
                            translateSpeed = TranslateAmount / currentTime;
                            yrotation = agent.transform.rotation.eulerAngles.y % 360;
                            if (yrotation > 180)
                            {
                                yrotation -= 360;
                            }

                            rotateySpeed = yrotation / currentTime;
                            while (currentTime >= 0)
                            {
                                agent.transform.Rotate(-RotateSpeed * Time.deltaTime, 0, 0,
                                    Space.Self);
                                agent.transform.Translate(0, 0, translateSpeed * Time.deltaTime, agent.transform);
                                currentTime -= Time.deltaTime;
                                yield return new WaitForFixedUpdate();
                            }

                            agent.transform.Rotate(0, 0, -yrotation, Space.Self);
                            break;
                        case TransformPosition.Cieling:
                            xrotation = (-180 - agent.transform.rotation.eulerAngles.x) % 360;
                            if (xrotation > 180)
                            {
                                xrotation -= 360;
                            }
                            currentTime = Mathf.Abs(xrotation / RotateSpeed);
                            translateSpeed = TranslateAmount / currentTime;
                            yrotation = (agent.transform.rotation.eulerAngles.y % 360) + 90 + (agent.transform.rotation.eulerAngles.x%90);
                            if (yrotation > 180)
                            {
                                yrotation -= 360;
                            }
                            zrotation = -90;
                            if (zrotation > 180)
                            {
                                zrotation -= 360;
                            }
                            while (currentTime >= 0)
                            {
                                agent.transform.Rotate(-1*RotateSpeed * Time.deltaTime, 0, 0/*-rotateySpeed * Time.deltaTime, -rotatezSpeed * Time.deltaTime*/,
                                    Space.Self);
                                agent.transform.Translate(0, 0, translateSpeed * Time.deltaTime, agent.transform);
                                currentTime -= Time.deltaTime;
                                yield return new WaitForFixedUpdate();
                            }
                            break;
                        case TransformPosition.Floor:
                            xrotation = (180 - agent.transform.rotation.eulerAngles.x) % 360;
                            if (xrotation > 180)
                            {
                                xrotation -= 360;
                            }
                            currentTime = Mathf.Abs(xrotation / RotateSpeed);
                            translateSpeed = TranslateAmount / currentTime;
                            yrotation = (agent.transform.rotation.eulerAngles.y % 360) + 90 + (agent.transform.rotation.eulerAngles.x%90);
                            if (yrotation > 180)
                            {
                                yrotation -= 360;
                            }
                            zrotation = -90;
                            if (zrotation > 180)
                            {
                                zrotation -= 360;
                            }
                            while (currentTime >= 0)
                            {
                                agent.transform.Rotate(-1*RotateSpeed * Time.deltaTime, 0, 0/*-rotateySpeed * Time.deltaTime, -rotatezSpeed * Time.deltaTime*/,
                                    Space.Self);
                                agent.transform.Translate(0, 0, translateSpeed * Time.deltaTime, agent.transform);
                                currentTime -= Time.deltaTime;
                                yield return new WaitForFixedUpdate();
                            }
                            break;                        
                        default:
                            break;
                    }

                    currentPos = newPos;
                    break;
                case TransformPosition.WallZ:
                    newPos = CheckTransformDirection();
                    switch (newPos)
                    {
                        case TransformPosition.WallX:
                            currentTime = 90 / RotateSpeed;
                            translateSpeed = TranslateAmount / currentTime;
                            yrotation = agent.transform.rotation.eulerAngles.y % 360;
                            if (yrotation > 180)
                            {
                                yrotation -= 360;
                            }
                            rotateySpeed = yrotation / currentTime;
                            while (currentTime >= 0)
                            {
                                agent.transform.Rotate(-RotateSpeed * Time.deltaTime, 0, 0,
                                    Space.Self);
                                agent.transform.Translate(0, 0, translateSpeed * Time.deltaTime, agent.transform);
                                currentTime -= Time.deltaTime;
                                yield return new WaitForFixedUpdate();
                            }

                            //agent.transform.Rotate(0, 0, -yrotation, Space.Self);
                            break;
                        case TransformPosition.Cieling:
                            xrotation = (-180 - agent.transform.rotation.eulerAngles.x) % 360;
                            if (xrotation > 180)
                            {
                                xrotation -= 360;
                            }
                            currentTime = Mathf.Abs(xrotation / RotateSpeed);
                            translateSpeed = TranslateAmount / currentTime;
                            yrotation = (agent.transform.rotation.eulerAngles.y % 360) + 90 + (agent.transform.rotation.eulerAngles.x%90);
                            if (yrotation > 180)
                            {
                                yrotation -= 360;
                            }
                            zrotation = -90;
                            if (zrotation > 180)
                            {
                                zrotation -= 360;
                            }
                            while (currentTime >= 0)
                            {
                                agent.transform.Rotate(-1*RotateSpeed * Time.deltaTime, 0, 0/*-rotateySpeed * Time.deltaTime, -rotatezSpeed * Time.deltaTime*/,
                                    Space.Self);
                                agent.transform.Translate(0, 0, translateSpeed * Time.deltaTime, agent.transform);
                                currentTime -= Time.deltaTime;
                                yield return new WaitForFixedUpdate();
                            }
                            //transform.Rotate(0, yrotation, zrotation);
                            break;                        
                        case TransformPosition.Floor:
                            xrotation = (180 - agent.transform.rotation.eulerAngles.x) % 360;
                            if (xrotation > 180)
                            {
                                xrotation -= 360;
                            }
                            currentTime = Mathf.Abs(xrotation / RotateSpeed);
                            translateSpeed = TranslateAmount / currentTime;
                            yrotation = (agent.transform.rotation.eulerAngles.y % 360) + 90 + (agent.transform.rotation.eulerAngles.x%90);
                            if (yrotation > 180)
                            {
                                yrotation -= 360;
                            }
                            zrotation = -90;
                            if (zrotation > 180)
                            {
                                zrotation -= 360;
                            }
                            while (currentTime >= 0)
                            {
                                agent.transform.Rotate(-1*RotateSpeed * Time.deltaTime, 0, 0/*-rotateySpeed * Time.deltaTime, -rotatezSpeed * Time.deltaTime*/,
                                    Space.Self);
                                agent.transform.Translate(0, 0, translateSpeed * Time.deltaTime, agent.transform);
                                currentTime -= Time.deltaTime;
                                yield return new WaitForFixedUpdate();
                            }
                            //transform.Rotate(0, yrotation, zrotation);
                            break;  
                        default:
                            break;
                    }

                    currentPos = newPos;
                    break;
                case TransformPosition.Cieling:
                    newPos = CheckTransformDirection();
                    currentTime = 90 / RotateSpeed;
                    translateSpeed = TranslateAmount / currentTime;
                    yrotation = agent.transform.rotation.eulerAngles.y % 360;
                    if (yrotation > 180)
                    {
                        yrotation -= 360;
                    }

                    rotateySpeed = yrotation / currentTime;
                    while (currentTime >= 0)
                    {
                        agent.transform.Rotate(-RotateSpeed * Time.deltaTime, 0, 0,
                            Space.Self);
                        agent.transform.Translate(0, 0, translateSpeed * Time.deltaTime, agent.transform);
                        currentTime -= Time.deltaTime;
                        yield return new WaitForFixedUpdate();
                    }

                    agent.transform.Rotate(0, 0, -yrotation, Space.Self);
                    currentPos = newPos;
                    break;
                default:
                    break;
            }


            agent.updateRotation = true;
            agent.enabled = true;
            agent.SetDestination(Destination.position);
            climbing = false;
        }

    }

    public TransformPosition CheckTransformDirection()
    {
        switch (currentPos)
        {
            case TransformPosition.Floor:
                if (Mathf.Abs(agent.transform.forward.z) > Mathf.Abs(agent.transform.forward.x))
                {
                    return TransformPosition.WallZ;
                }
                else
                {
                    return TransformPosition.WallX;
                }
            case TransformPosition.WallX:
                if (Mathf.Abs(agent.transform.forward.z) > Mathf.Abs(agent.transform.forward.y))
                {
                    return TransformPosition.WallZ;
                }
                else
                {
                    if (agent.transform.forward.y > 0)
                    {
                        return TransformPosition.Cieling;
                    }

                    return TransformPosition.Floor;
                }
            case TransformPosition.WallZ:
                if (Mathf.Abs(agent.transform.forward.x) > Mathf.Abs(agent.transform.forward.y))
                {
                    return TransformPosition.WallX;
                }
                else
                {
                    if (agent.transform.forward.y > 0)
                    {
                        return TransformPosition.Cieling;
                    }

                    return TransformPosition.Floor;
                }
            case TransformPosition.Cieling:
                if (Mathf.Abs(agent.transform.forward.z) > Mathf.Abs(agent.transform.forward.x))
                {
                    return TransformPosition.WallZ;
                }
                else
                {
                    return TransformPosition.WallX;
                }
        }

        return TransformPosition.Floor;
    }

}
