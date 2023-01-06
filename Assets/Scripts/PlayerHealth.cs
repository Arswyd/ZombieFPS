using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;

    DeathHandler deathHandler;
    PostProcessingHandler postProcessingHandler;

    void Awake() 
    {
        deathHandler = GetComponent<DeathHandler>();
        postProcessingHandler = FindObjectOfType<PostProcessingHandler>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        postProcessingHandler.ShowDamageVignette();
        if(health <= 0 && deathHandler != null)
        {
            deathHandler.HandleDeath();
        }
    }
}
