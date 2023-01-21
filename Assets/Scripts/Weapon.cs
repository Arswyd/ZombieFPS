using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera FPCamera;
    [SerializeField] float shortrange = 50f;
    [SerializeField] float longrange = 100f;
    [SerializeField] float shortRangeDamage = 50f;
    [SerializeField] float longRangeDamage = 50f;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] float timeBeforeReload = 0.5f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject enemyHitEffect;
    [SerializeField] Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] AudioClip shotSFX;
    [SerializeField] AudioClip reloadSFX;
    [SerializeField] bool zoomOutWhenReloading;

    private StarterAssetsInputs starterAssetsInputs;
    Animator animator;
    AudioSource audioSource;
    bool canShoot = true;
    bool isReloading = false;

    void Awake() 
    {
        starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(starterAssetsInputs.fire && canShoot)
        {
           StartCoroutine(Shoot()); 
        }
        starterAssetsInputs.fire = false;

        DisplayAmmo();
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        if (ammoSlot.GetCurrentAmmoAmount(ammoType) > 0)
        {
            animator.SetTrigger("Fire");
            StartCoroutine(PlayClip(shotSFX, reloadSFX));
            PlayMuzzleFlash();
            ProcessRaycast();
            ammoSlot.ReduceCurrentAmmo(ammoType);
        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
        if (zoomOutWhenReloading)
            isReloading = false;
    }

    void DisplayAmmo()
    {
        ammoText.text = ammoSlot.GetCurrentAmmoAmount(ammoType).ToString();
    }

    void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    void ProcessRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, longrange))
        {
            CreateHitImpact(hit);
            EnemyProcessHit target = hit.transform.GetComponent<EnemyProcessHit>();
            if (target != null)
            {
                target.ProcessHit(hit.distance >= shortrange ? longRangeDamage : shortRangeDamage);
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

    IEnumerator PlayClip(AudioClip shotClip, AudioClip reloadClip)
    {
        if(shotClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(shotClip);
        }

        yield return new WaitForSeconds(timeBeforeReload);

        if(reloadClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(reloadClip);
        }
        
        if (zoomOutWhenReloading)
            isReloading = true;
    }

    public bool GetIsReloading()
    {
        return isReloading;
    }
}
