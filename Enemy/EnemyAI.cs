using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float speedWalk;
    [SerializeField]
    private float speedRun;

    [SerializeField]
    private GameObject deadEffect;
    [SerializeField]
    private ActionEvent action;

    [SerializeField]
    private AudioSource enemySound;
    [SerializeField]
    private AudioClip deadSound;

    private NavMeshAgent agent;
    Vector3 velocity = Vector3.zero;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
    }

    void FixedUpdate()
    {
        float d = InputHandler.Delta;
        agent.SetDestination(target.position);
        transform.position = Vector3.SmoothDamp(transform.position, agent.nextPosition, ref velocity, d);
    }

    public void GetDamage()
    {
        deadEffect.SetActive(true);

        enemySound.loop = false;
        enemySound.clip = deadSound;
        enemySound.Play();

        action?.CallEvent();
    }
}
