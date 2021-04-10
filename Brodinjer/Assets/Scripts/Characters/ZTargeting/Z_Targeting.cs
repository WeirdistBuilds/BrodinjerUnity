using System.Linq; //Sorting for Orderby
using System.Collections.Generic;
using UnityEngine;

public class Z_Targeting : MonoBehaviour
{
    public float m_Angle;
    [HideInInspector]
    public bool isTargeting;
    public CameraRotationManager cameraRot;
    public Transform forwardDirection;

    public List<GameObject> objInRange = new List<GameObject>();
    public GameObject objClosest;

    private List<GameObject> objsInTarget = new List<GameObject>();
    private List<GameObject> objTargetable = new List<GameObject>();
    private int numoOfTargets;
    private bool TargetButton;
    private bool AxisRight, AxisLeft;
    private List<GameObject> sortedList;
    private Vector3 targetDirection;
    private Vector2 cameraForward, targetdir;
    private float angle;
    private GameObject nextTarget;
    private int targetIndex;
    private Transform mainCamera;

    public BoolData paused;
    private RaycastHit hit;
    public LayerMask ignoreLayers;

    private void Start()
    {
        mainCamera = Camera.main.transform;
        isTargeting = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.GetComponent<TargetObject>()) != null)
        {
            objsInTarget.Add(other.gameObject);
            numoOfTargets++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.GetComponent<TargetObject>()) != null)
        {
            other.GetComponent<TargetObject>().target.SetActive(false);
            objsInTarget.Remove(other.gameObject);
            numoOfTargets++;
        }
    }
    private void Update()
    {
        if (!paused.value)
        {
            //Target Button Pressed
            bool isDown = Input.GetButtonDown("Target");

            //If not Targeting - Target Button is Off (not targeting)
            if (objClosest == null)
            {
                TargetButton = false;
                isTargeting = false;
            }

            //Remove any Null Objects
            objsInTarget = CopyAndRemove(objsInTarget);
            //Check if angle is correct
            objInRange = InAngle(objsInTarget);

            //If you pressed the target button and there are possible targets
            if (isDown && objInRange.Count > 0)
            {
                sortedList = objInRange.OrderBy(gameobj => GetAngle(gameobj.transform, mainCamera)).ToList();
                objInRange = CopyAndRemove(objInRange, sortedList);
                UnTarget(objClosest);
                objClosest = objInRange.First();
                Target(objClosest);

                //Toggle Target On / Off
                TargetButton = !TargetButton;
            }

            //if currently targeting
            if (TargetButton && numoOfTargets > 0 && objClosest != null)
            {
                if (GetAngle(objClosest.transform, mainCamera) < Mathf.Abs(m_Angle))
                {
                    isTargeting = true;
                    //If you move to the Right
                    if (Input.GetAxisRaw("TargetChange") > 0)
                    {
                        //If the Axis Change is not currently Right (don't move more than once per click)
                        if (AxisRight == false)
                        {
                            sortedList = objInRange.OrderBy(go => GetSignedAngle(go.transform, mainCamera)).ToList();
                            objInRange = CopyAndRemove(objInRange, sortedList);

                            //If there are more objects to the Right;
                            if ((targetIndex = objInRange.IndexOf(objClosest) - 1) >= 0)
                            {
                                UnTarget(objClosest);
                                nextTarget = objInRange[targetIndex];
                                if (GetAngle(nextTarget.transform, mainCamera) < Mathf.Abs(m_Angle))
                                {
                                    objClosest = nextTarget;
                                }
                                Target(objClosest);
                                AxisRight = !AxisRight;
                            }
                        }
                    }
                    //If Move Left
                    else if (Input.GetAxisRaw("TargetChange") < 0)
                    {
                        if (AxisLeft == false)
                        {
                            sortedList = objInRange.OrderBy(go => GetSignedAngle(go.transform, mainCamera)).ToList();
                            objInRange = CopyAndRemove(objInRange, sortedList);

                            //If objects to Left
                            if ((targetIndex = objInRange.IndexOf(objClosest) + 1) < objInRange.Count)
                            {
                                UnTarget(objClosest);
                                nextTarget = objInRange[targetIndex];

                                if (GetAngle(nextTarget.transform, mainCamera) < Mathf.Abs(m_Angle))
                                {
                                    objClosest = nextTarget;
                                }
                                Target(objClosest);
                                AxisLeft = !AxisLeft;
                            }
                        }
                    }
                    else
                    {
                        AxisRight = false;
                        AxisLeft = false;
                    }
                }
                else
                {
                    isTargeting = false;
                }
            }
            else
            {
                isTargeting = false;
                UnTarget(objClosest);
                objClosest = null;
            }
        }
    }
    private float GetAngle(Transform target, Transform point)
    {
        targetDirection = target.transform.position - point.position;
        cameraForward = new Vector2(point.forward.x, point.forward.z);
        targetdir = new Vector2(targetDirection.x, targetDirection.z);
        angle = Vector2.Angle(cameraForward, targetdir);
        //angle = Vector3.Angle(point.forward, targetDirection);
        return angle;
    }

    private float GetSignedAngle(Transform target, Transform point)
    {
        targetDirection = target.transform.position - mainCamera.position;
        cameraForward = new Vector2(point.forward.x, point.forward.z);
        targetdir = new Vector2(targetDirection.x, targetDirection.z);
        angle = Vector2.SignedAngle(cameraForward, targetdir);
        //angle = Vector3.SignedAngle(point.forward, targetDirection, Vector3.up);
        return angle;
    }

    private List<GameObject> CopyAndRemove(List<GameObject> objs, List<GameObject> copyList = null)
    {
        for (int i = 0; i < objs.Count; ++i)
        {
            if (copyList != null)
            {
                objs[i] = copyList[i];
            }
            if (objs[i] != null && Physics.Raycast(transform.forward, objs[i].transform.position, Vector3.Distance(transform.position, objs[i].transform.position), ignoreLayers)) {
                objs.RemoveAt(i);
                numoOfTargets--;
            }
            else if (objs[i] == null || !objs[i].activeInHierarchy)
            {
                objs.RemoveAt(i);
                numoOfTargets--;
            }
        }
        return objs;
    }

    private List<GameObject> InAngle(List<GameObject> objs)
    {
        return objs;
    }

    private void UnTarget(GameObject obj)
    {
        if (obj != null)
        {
            TargetObject temp = obj.GetComponent<TargetObject>();
            if (temp != null && temp.target != null)
                temp.target.SetActive(false);
        }
    }
    private void Target(GameObject obj)
    {
        if (obj != null)
        {
            TargetObject temp = obj.GetComponent<TargetObject>();
            if (obj != null && temp != null && temp.target != null)
                temp.target.SetActive(true);
        }
    }
}



