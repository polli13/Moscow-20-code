using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    private float timer;

    void Start()
    {
        Destroy(gameObject, timer);
    }

}
