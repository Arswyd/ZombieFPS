using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProcessHit : MonoBehaviour
{
    [SerializeField] float damageMultiplier = 1f;

    EnemyHealth enemyHealth;
    
    void Awake()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    public void ProcessHit(float damage)
    {
        Debug.Log(damageMultiplier.ToString());
        enemyHealth.TakeDamage(damage * damageMultiplier);
    }
}
