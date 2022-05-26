using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeapon = 0;

    StarterAssetsInputs starterAssetsInputs;
    Weapon weaponScript;
    WeaponZoom weaponZoom;

    void Awake() 
    {
        starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        weaponScript = GetComponentInChildren<Weapon>();
        weaponZoom = GetComponentInChildren<WeaponZoom>();
    }
    
    void Start()
    {
        SetWeaponActive();
    }

    void Update()
    {
        if(!weaponScript.GetCanShoot() || weaponZoom.GetZoomedState())
        {
            return;
        }

        int previousWeapon = currentWeapon;

        ProcessKeyInput();
        ProcessScrollWheel();

        if(previousWeapon != currentWeapon)
        {
            SetWeaponActive();
            weaponScript = GetComponentInChildren<Weapon>();
            weaponZoom = GetComponentInChildren<WeaponZoom>();
        }
    }

    void ProcessScrollWheel()
    {
        if(starterAssetsInputs.scroll > 0)
        {
            if(currentWeapon >= transform.childCount - 1)
            {
                currentWeapon = 0;
            }
            else
            {
                currentWeapon++;
            }
        }
        if(starterAssetsInputs.scroll < 0)
        {
            if(currentWeapon <= 0)
            {
                currentWeapon = transform.childCount - 1;
            }
            else
            {
                currentWeapon--;
            } 
        }
    }

    void ProcessKeyInput()
    {
        if(starterAssetsInputs.switchPistol)
        {
            currentWeapon = 0;
            starterAssetsInputs.switchPistol = false;
        }
        if(starterAssetsInputs.switchShotgun)
        {
            currentWeapon = 1;
            starterAssetsInputs.switchShotgun = false;
        }
        if(starterAssetsInputs.switchSniper)
        {
            currentWeapon = 2;
            starterAssetsInputs.switchSniper = false;
        }
    }

    void SetWeaponActive()
    {
        int weaponIndex = 0;

        foreach(Transform weapon in transform)
        {
            if (weaponIndex == currentWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            weaponIndex++;
        }
    }
}
