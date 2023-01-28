using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float chaseRange = 10f;
    [SerializeField] float alertedChaseRange = 20f;
    [SerializeField] float alertDecreaseSpeed = 2f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 4f;
    [SerializeField] float waitLenght = 4f;
    [SerializeField] bool isWaiting = false;
    [SerializeField] AudioClip screamSFX;


    NavMeshAgent navMeshAgent;
    Transform target;
    Animator animator;
    EnemyHealth enemyHealth;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    bool isAlerted = false;
    AudioSource audioSource;
    AudioClip originalSFX;
    EnemyGroupAlerter enemyGroupAlerter;
    float originalChaseRange;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        target = FindObjectOfType<PlayerHealth>().transform;
        audioSource = GetComponent<AudioSource>();
        enemyGroupAlerter = GetComponentInParent<EnemyGroupAlerter>();
        originalChaseRange = chaseRange;
    }

    void Start()
    {
        if (isWaiting)
        {
            audioSource.enabled = false;
        }
        else
        {
            audioSource.time = UnityEngine.Random.Range(0f,30f);
            StartCoroutine(PlayDelayedAnimation(UnityEngine.Random.Range(0f,2f)));
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
            GetProvoked(false);
        }
        if(isAlerted)
        {
            if(chaseRange >= originalChaseRange)
            {
                chaseRange = chaseRange - Time.deltaTime * alertDecreaseSpeed;
            }
            else
            {
                isAlerted = false;
                chaseRange = originalChaseRange;
            }
        }
    }

    public void OnDamageTaken(bool isDead)
    {
        if (isProvoked) { return; }
            GetProvoked(isDead);
    }

    void GetProvoked(bool isDead)
    {     
        isProvoked = true;
        if(!isAlerted)
        {
            enemyGroupAlerter.AlertEnemyGroup();
            if(!isDead)
                StartCoroutine(StartAwakeningScream());
        }
    }

    void GetAlerted()
    {
        isAlerted = true;
        chaseRange = alertedChaseRange;
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

    IEnumerator PlayDelayedAnimation(float delay) 
    {
        yield return new WaitForSeconds(delay);
        if(!enemyHealth.IsDead())
        {
            animator.Play("Idle");
        }
    }

    IEnumerator StartAwakeningScream() 
    {
        if(isWaiting)
            yield return new WaitForSeconds(0.5f);

        originalSFX = audioSource.clip;
        audioSource.Stop();
        audioSource.time = 0f;
        audioSource.clip = screamSFX;
        audioSource.loop = false;
        audioSource.enabled = true;
        if(!enemyHealth.IsDead())
            audioSource.Play();

        yield return new WaitForSeconds(2.5f);

        audioSource.Stop();
        audioSource.clip = originalSFX;
        audioSource.loop = true;
        if(!enemyHealth.IsDead())
            audioSource.Play();
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
