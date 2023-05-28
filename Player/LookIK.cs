using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookIK : MonoBehaviour
{
    public Transform target;
    [SerializeField]
    private bool autoFindTarget;

    [SerializeField]
    private float weight = 1f;
    [SerializeField]
    private float headWeight = 1f;
    [SerializeField]
    private float bodyWeight = 1f;
    [SerializeField]
    private float clampWeight = 1f;

    [SerializeField]
    private Animator anim;

    private void Start()
    {
        if(autoFindTarget && target == null)
            target = GameObject.FindGameObjectWithTag("Target").transform;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetLookAtPosition(target.position);
        anim.SetLookAtWeight(weight, bodyWeight, headWeight, 1, clampWeight);
    }

    public void ChangeTarget(Transform _target)
    {
        target = _target;
    }
}
