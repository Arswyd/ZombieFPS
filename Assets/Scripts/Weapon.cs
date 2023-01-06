using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera FPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 50f;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject enemyHitEffect;
    [SerializeField] Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;

    private StarterAssetsInputs starterAssetsInputs;
    Animator animator;
    bool canShoot = true;

    void Awake() 
    {
        starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(starterAssetsInputs.fire && canShoot)
        {
           StartCoroutine(Shoot()); 
        }
        starterAssetsInputs.fire = false;
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        if(ammoSlot.GetCurrentAmmoAmount(ammoType) > 0)
        {
            animator.SetTrigger("Fire");
            PlayMuzzleFlash();
            ProcessRaycast();
            ammoSlot.ReduceCurrentAmmo(ammoType);
        }

        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    void ProcessRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range))
        {
            CreateHitImpact(hit);
            EnemyProcessHit target = hit.transform.GetComponent<EnemyProcessHit>();
            if (target != null)
            {
                target.ProcessHit(damage);
            }
        }
        else
        {
            return;
        }
    }

    void CreateHitImpact(RaycastHit hit)
    {
        GameObject instance = Instantiate(hit.collider.transform.tag == "Enemy" ? enemyHitEffect : hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(instance, 1);
    }

    public bool GetCanShoot()
    {
        return canShoot;
    }
}
