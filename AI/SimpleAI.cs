using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Transform target;

    Vector3 velocity = Vector3.zero;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
    }

    public void FixedUpdate()
    {
        float d = InputHandler.Delta;

        agent.SetDestination(target.position);
        transform.position = Vector3.SmoothDamp(transform.position, agent.nextPosition, ref velocity, d); //0.1f
    }
}
