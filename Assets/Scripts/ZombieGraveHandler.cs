using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGraveHandler : MonoBehaviour
{
    [SerializeField] float awakeningTime = 0.1f;
    [SerializeField] float gaveDestroyDelay = 0.3f;
    [SerializeField] ParticleSystem particleFX;
    EnemyAI enemyAI;
    bool hasAwakened;

    void Awake()
    {
        enemyAI = transform.parent.GetComponentInChildren<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAwakened && enemyAI.GetIsProvoked())
        {
            hasAwakened = true;
            StartCoroutine(BeginAwakening());
        }
    }

    IEnumerator BeginAwakening()
    {
        yield return new WaitForSeconds(awakeningTime);
        particleFX.Play();
        yield return new WaitForSeconds(gaveDestroyDelay);
        Destroy(gameObject);
    }
}
