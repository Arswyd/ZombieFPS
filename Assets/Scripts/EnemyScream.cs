using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScream : MonoBehaviour
{
    AudioSource audioSource;
    Animator animator;
    [SerializeField] AudioClip screamSFX;

    [SerializeField] bool debug;

    bool isScreaming;
    float previousBlend;
    AudioClip originalSFX;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void ScreamingEvent()
    {
        if (animator.GetFloat("Blend") < 2.5f) {return;}
    
        StartCoroutine(StartScreaming());
    }

    IEnumerator StartScreaming()
    {
        originalSFX = audioSource.clip;
        audioSource.Stop();
        audioSource.clip = screamSFX;
        audioSource.loop = false;
        audioSource.Play();

        yield return new WaitForSeconds(2.5f);

        audioSource.Stop();
        audioSource.clip = originalSFX;
        audioSource.loop = true;
        audioSource.Play();
    }
}
