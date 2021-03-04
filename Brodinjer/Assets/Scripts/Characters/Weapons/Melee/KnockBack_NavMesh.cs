using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class KnockBack_NavMesh : MonoBehaviour
{
    public float thrust; //force
    public float knockTime; //time
    private Vector3 difference;
    private Rigidbody enemyRB;
    private Enemy_Manager enemyManager;
    public Transform BaseObj;
    private Timed_Event Reset;
    private NavMeshAgent agent;
    public UnityEvent OnKnockback, OnKnockbackEnd;
    private bool running;
    private float currentTime;

    private void Start()
    {
        if (BaseObj == null)
        {
            BaseObj = transform;
        }
    }

    
    protected virtual void OnTriggerEnter(Collider other)
    {
        agent = other.GetComponent<NavMeshAgent>();
        if (agent == null)
            agent = other.GetComponentInParent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
            /*difference = agent.transform.position - BaseObj.position;
            difference.y = 0;
            difference = (difference.normalized * thrust);*/
            difference = agent.transform.position + transform.forward;
            if (!running)
            {
                OnKnockback.Invoke();
                StartCoroutine(KnockCo(agent, difference));
            }
        }

    }

    protected virtual IEnumerator KnockCo(NavMeshAgent character, Vector3 impact)
    {
        running = true;
        if (character != null)
        {
            currentTime = knockTime;
            while (currentTime > 0)
            {
                character.transform.Translate(impact * Time.deltaTime);
                //character.Move(impact * Time.deltaTime);
                //impact = Vector3.Lerp(impact, Vector3.zero, knockTime * Time.deltaTime);
                currentTime -= Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

        agent.enabled = true;
        OnKnockbackEnd.Invoke();
        running = false;
    }

    private void OnDisable()
    {
        running = false;
    }

}
