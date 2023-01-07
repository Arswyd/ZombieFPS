using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 4f;
    [SerializeField] float waitLenght = 4f;
    [SerializeField] bool isWaiting = false;

    [SerializeField] float maxAnimationCycleOffset = 0.5f;

    [SerializeField] AudioClip screamSFX;


    NavMeshAgent navMeshAgent;
    Transform target;
    Animator animator;
    EnemyHealth enemyHealth;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    AudioSource audioSource;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetFloat("CycleOffset", UnityEngine.Random.Range(0, maxAnimationCycleOffset));
        enemyHealth = GetComponent<EnemyHealth>();
        target = FindObjectOfType<PlayerHealth>().transform;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (isWaiting)
        {
            audioSource.enabled = false;
        }
        else
        {
            audioSource.PlayDelayed(UnityEngine.Random.Range(0f,10f));
        }
    }

    void Update()
    {
        if(enemyHealth.IsDead())
        {
            enabled = false;
            navMeshAgent.enabled = false;
            return;
        }
        if(animator.GetBool("isWalking"))
        {
            navMeshAgent.speed = walkSpeed;
        }
        else
        {
            navMeshAgent.speed = runSpeed;
        }
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvoked)
        { 
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
            audioSource.enabled = true;
            audioSource.PlayOneShot(screamSFX);
            audioSource.Play();
        }
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    void EngageTarget()
    {
        if(!isWaiting)
        {
            FaceTarget();
        }
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f > 0.01f) { return; }
            animator.SetBool("attack", false);
            ChaseTarget();
        }
        else
        {
            animator.SetBool("attack", true);
            AttackTarget();
        }
    }

    void ChaseTarget()
    {
        animator.SetTrigger("move");
        if(isWaiting)
        {
            StartCoroutine(WaitForAttack());
        }
        else
        {
            navMeshAgent.SetDestination(target.position);
        }
    }

    IEnumerator WaitForAttack() 
    {
        yield return new WaitForSeconds(waitLenght);
        isWaiting = false;
    }

    void AttackTarget()
    {
        //---
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    public bool GetIsProvoked()
    {
        return isProvoked;
    }
}
