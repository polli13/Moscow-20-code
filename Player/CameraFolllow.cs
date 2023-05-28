using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFolllow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private bool canFollow = true;
    [SerializeField]
    private bool smoothFollow = true;
    public bool canZoom = true;

    [SerializeField]
    private Transform cameraMain;
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;

    [SerializeField]
    private float startZoomSpeed;
    [SerializeField]
    private float endZoomSpeed;

    private float zoomSpeed;
    private Transform currentPos;

    public void FixedTick(float d)
    {
        if (canFollow)
            FollowTarget(d);

        if (!canZoom) return;
        if (PlayerController.m_PlayerController.CheckVelocityStep())
        {
            currentPos = endPoint;
            zoomSpeed = endZoomSpeed;
        }
        else
        {
            currentPos = startPoint;
            zoomSpeed = startZoomSpeed;
        }

        cameraMain.position = Vector3.Lerp(cameraMain.position, currentPos.position, Time.deltaTime * zoomSpeed);
    }

    public void CanZoom(bool _can)
    {
        canZoom = _can;
    }

    private void FollowTarget(float d)
    {
        Vector3 targetPos;

        if (smoothFollow)
        {
            targetPos = Vector3.Lerp(transform.position, target.position, moveSpeed * d);
        }
        else
            targetPos = target.position;

        transform.position = targetPos;
    }
}
