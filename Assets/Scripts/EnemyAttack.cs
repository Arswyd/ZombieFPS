using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float attackOffset = 1f;
    [SerializeField] float damage = 40f;

    PlayerHealth target;
    float distanceToTarget = Mathf.Infinity;
    NavMeshAgent navMeshAgent;
    EnemyHealth enemyHealth;
    

    void Awake()
    {
        target = FindObjectOfType<PlayerHealth>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update() 
    {
        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
    }

    public void AttackHitEvent()
    {
        if(target != null && !enemyHealth.IsDead() && distanceToTarget <= navMeshAgent.stoppingDistance + attackOffset)
        {
            target.TakeDamage(damage);
        }
    }
}
