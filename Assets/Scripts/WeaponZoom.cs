using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class WeaponZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera zoomCamera;
    //[SerializeField] float zoomedOutFOV = 60f;
    //[SerializeField] float zoomedInFOV = 20f;
    [SerializeField] float zoomedInSensitivity = 0.5f;

    bool zoomedInToggle = false;
    float zoomedOutSensitivity;

    private StarterAssetsInputs starterAssetsInputs;
    private FirstPersonController firstPersonController;

    void Awake() 
    {
        starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        firstPersonController = FindObjectOfType<FirstPersonController>();
    }

    void Start() 
    {
        zoomedOutSensitivity = firstPersonController.RotationSpeed;
    }

    void Update() 
    {
        if (zoomedInToggle == starterAssetsInputs.zoom)
        {
            return;
        }

        if (starterAssetsInputs.zoom)
        {
            zoomedInToggle = true;
            zoomCamera.gameObject.SetActive(true);
            firstPersonController.RotationSpeed = zoomedInSensitivity;
        }
        else
        {
            zoomedInToggle = false;
            zoomCamera.gameObject.SetActive(false);
            firstPersonController.RotationSpeed = zoomedOutSensitivity;
        }
    }

    public bool GetZoomedState()
    {
        return zoomedInToggle;
    }
}
