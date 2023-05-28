using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogAI : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rbPlayer;

    private Rigidbody rb;
    private NavMeshAgent agent;
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float speedWalk;
    [SerializeField]
    private float speedRun;
    [SerializeField]
    private float changeSpeedPercent;
    private float currentSpeed;

    [SerializeField]
    private float stayRadius;
    [SerializeField]
    private float runRadius;

    [SerializeField]
    private DogState states;

    Vector3 velocity = Vector3.zero;
    bool staying = false;
    bool running = false;

    private float waitTimer;
    [SerializeField]
    private float waitToSit;
    [SerializeField]
    private float waitToLie;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
    }

    public void FixedUpdate()
    {
        float d = InputHandler.Delta;
        float _forward = agent.desiredVelocity.magnitude;

        agent.SetDestination(target.position);
        transform.position = Vector3.SmoothDamp(transform.position, agent.nextPosition, ref velocity, d);

        agent.speed = Mathf.Lerp(agent.speed, currentSpeed, Time.deltaTime * changeSpeedPercent);
        anim.SetFloat("Speed", _forward, 0.2f, Time.fixedDeltaTime);

        CheckDistance();
        CheckAnimation();
    }

    private void CheckDistance()
    {
        float _distance = Vector3.Distance(transform.position, rbPlayer.transform.position);

        if (_distance < stayRadius)
        {
            if (!staying && states != DogState.Wait)
            {
                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    staying = true;
                    ChangeState(DogState.Wait);
                }
            }
        }
        else if (_distance > stayRadius)
        {
            if (_distance > runRadius && !running)
            {
                running = true;
                ChangeState(DogState.Run);
            }
            else if (running)
            {
                running = false;
                staying = false;
                ChangeState(DogState.Walk);
            }
        }
    }

    private void CheckAnimation()
    {
        if(states == DogState.Wait)
        {
            waitTimer += Time.deltaTime;

            if(waitTimer > waitToSit && waitTimer < waitToLie)
            {
                anim.SetBool("Sit", true);
            }
            else if(waitTimer > waitToLie)
            {
                anim.SetBool("Lie", true);
                anim.SetBool("Sit", false);
            }
        }
    }

    private void ChangeState(DogState _state)
    {
        states = _state;
        
        switch(_state)
        {
            case DogState.Wait:
                agent.isStopped = true;
                currentSpeed = 0;
                waitTimer = 0;
                break;
            case DogState.Walk:
                agent.isStopped = false;
                currentSpeed = speedWalk;
                anim.SetBool("Sit", false);
                anim.SetBool("Lie", false);
                break;
            case DogState.Run:
                agent.isStopped = false;
                currentSpeed = speedRun;
                anim.SetBool("Sit", false);
                anim.SetBool("Lie", false);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, runRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stayRadius);
    }

    public enum DogState
    {
        Wait,
        Walk,
        Run
    }
}
