using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private int priorityBoostAmount = 10;
    [SerializeField]
    private Canvas thirdPersonCanvas;
    [SerializeField]
    private Canvas aimCanvas;

    public CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    private void Awake()
    {
        //virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
        if (aimCanvas != null && thirdPersonCanvas != null)
        {
            aimCanvas.enabled = true;
            thirdPersonCanvas.enabled = false;
        }
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        if (aimCanvas != null && thirdPersonCanvas != null)
        {
            aimCanvas.enabled = false;
            thirdPersonCanvas.enabled = true;
        }
    }
}
