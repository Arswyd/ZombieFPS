using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] AudioClip audioClip;

    DeathHandler deathHandler;
    PostProcessingHandler postProcessingHandler;
    AudioSource audioSource;

    void Awake() 
    {
        deathHandler = GetComponent<DeathHandler>();
        postProcessingHandler = FindObjectOfType<PostProcessingHandler>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        audioSource.PlayOneShot(audioClip);
        postProcessingHandler.ShowDamageVignette();
        if(health <= 0 && deathHandler != null)
        {
            deathHandler.HandleDeath();
        }
    }
}
