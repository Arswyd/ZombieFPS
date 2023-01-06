using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGraveHandler : MonoBehaviour
{
    [SerializeField] float awakeningTime = 2f;
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
        //particleFX.Play();
        yield return new WaitForSeconds(awakeningTime);
        particleFX.Play();
        Destroy(gameObject);
    }
}
