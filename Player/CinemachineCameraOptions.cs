using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraOptions : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualMainCamera;
    private CinemachineVirtualCamera currentCam;

    [SerializeField]
    private float fovSpeed;

    void Start()
    {
        currentCam = virtualMainCamera;
    }

    public void ChangeFOV(float _fov)
    {
        virtualMainCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualMainCamera.m_Lens.FieldOfView, _fov, Time.deltaTime * fovSpeed);
    }


    public void ChangeCamera(CinemachineVirtualCamera _cam)
    {
        currentCam.gameObject.SetActive(false);
        _cam.gameObject.SetActive(true);
    }
}
