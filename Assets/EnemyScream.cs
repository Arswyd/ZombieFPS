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
        if (debug)
            Debug.Log(animator.GetFloat("Blend"));

        if (animator.GetFloat("Blend") < 2.5f) {return;}
    
        StartCoroutine(StartScreaming());
        //audioSource.PlayOneShot(screamSFX);
    }

    IEnumerator StartScreaming()
    {
        audioSource.Pause();
        audioSource.PlayOneShot(screamSFX);

        yield return new WaitForSeconds(10f);

        audioSource.UnPause();
    }
}
