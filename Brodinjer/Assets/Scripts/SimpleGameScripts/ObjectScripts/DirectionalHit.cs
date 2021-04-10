using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalHit : Trigger_Event_Base
{
    public Transform player;
    public Animator anim;
    public string AnimDirectionName = "Direction";
    public string AnimHitName = "Hit";
    private bool checking;
    public float InBetweenTime;

    private void Awake()
    {
        if (player == null)
            player = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Start()
    {
        checking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!checking)
        {
            checking = true;
            StartCoroutine(CheckTrigger(other));
        }
    }

    public override IEnumerator CheckTrigger(Collider coll)
    {
        switch (checksFor)
        {
            case Check.Layer:
                
                if (layer == (layer | (1 << coll.gameObject.layer)))
                {
                    yield return new WaitForSeconds(waitTime);
                    anim.SetFloat(AnimDirectionName, GetDirection());
                    anim.SetTrigger(AnimHitName);
                    if(coll.tag == "Arrow")
                        coll.gameObject.layer = 0;
                    Event.Invoke();

                }
                break;
            case Check.Name:
                if (coll.gameObject.name.Contains(objName))
                {
                    yield return new WaitForSeconds(waitTime);
                    anim.SetFloat(AnimDirectionName, GetDirection());
                    anim.SetTrigger(AnimHitName);
                    if(coll.tag == "Arrow")
                        coll.gameObject.layer = 0;
                    Event.Invoke();

                }
                break;
            case Check.Tag:
                if (coll.CompareTag(tagName))
                {
                    yield return new WaitForSeconds(waitTime);
                    anim.SetFloat(AnimDirectionName, GetDirection());
                    anim.SetTrigger(AnimHitName);
                    if(coll.tag == "Arrow")
                        coll.gameObject.layer = 0;
                    Event.Invoke();
                }
                break;
        }
        yield return new WaitForSeconds(InBetweenTime);
        checking = false;
    }

    public virtual float GetDirection()
    {
        Vector3 collisionposition = player.position;
        collisionposition.y = 0;
        Vector3 transformposition = transform.position;
        transformposition.y = 0;
        Vector3 target = collisionposition - transformposition;
        float angle = Vector3.Angle(target, transform.forward);
        Vector3 crossProduct = Vector3.Cross(target, transform.forward);
        if (crossProduct.y < 0)
        {
            angle = -angle;
        }

        angle /= 360;
        angle += .5f;
        return angle;
    }
}
