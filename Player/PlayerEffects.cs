using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField]
    private AudioSource playerStepSource;

    [SerializeField]
    private AudioClip[] playerStepClips;

    [SerializeField]
    private SoundsPlayer sounds;

    [SerializeField]
    private UnityEngine.AI.NavMeshAgent agent;

    [SerializeField] private Cinemachine.CinemachineImpulseSource _impulse;

    private FlashlightSystem flashlight;
    private Animator anim;

    [SerializeField]
    private AudioClip[] playerReactClips;

    [SerializeField]
    private SoundsPlayer headSounds;

    private void Awake()
    {
        flashlight = FindObjectOfType<FlashlightSystem>();
        anim = GetComponent<Animator>();
    }

    public void PlayStep(int _i)
    {
        if (!PlayerController.m_PlayerController.CheckVelocityStep()) return;
        if (PlayerController.m_PlayerController.states.isRunning && _i == 0) return;

        var _clip = playerStepClips[Random.Range(0, playerStepClips.Length)];
        sounds.PlaySound(_clip);
    }

    public void JustStep()
    {
        if (!agent || agent.velocity.magnitude < 0.1f) return;
        var _clip = playerStepClips[Random.Range(0, playerStepClips.Length)];

        sounds.PlaySound(_clip);
    }

    public void LightAttackEffect() 
    {
        flashlight.LightAttack();
        _impulse.GenerateImpulse();
        anim.CrossFade(StaticStrings.Shoot, 0.2f);

        var _clip = playerReactClips[Random.Range(0, playerReactClips.Length)];
        headSounds.PlaySound(_clip);
    }
}
