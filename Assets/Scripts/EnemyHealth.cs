using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    [SerializeField] AudioClip deathSFX;
    AudioSource audioSource;

    bool isDead = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        if(hitPoints <= 0)
        {
            Die();
        }
        BroadcastMessage("OnDamageTaken", isDead);
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        audioSource.Stop(); //enabled = false;
        audioSource.PlayOneShot(deathSFX, 2f);
        Debug.Log("asd");
        GetComponent<Animator>().SetTrigger("die");
    }
}
