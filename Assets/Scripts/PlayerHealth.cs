using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;

    DeathHandler deathHandler;

    void Awake() 
    {
        deathHandler = GetComponent<DeathHandler>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0 && deathHandler != null)
        {
            deathHandler.HandleDeath();
        }
    }
}
