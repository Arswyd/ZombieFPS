using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlBehaviour : StateMachineBehaviour
{
    [SerializeField] float timeUntilCrawling;
    int numberOfBoredAnimation = 1;
    bool isCrawling;
    float idleTime;
    int boredAnimation;
   
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       ResetIdle(); 
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(isCrawling == false) 
       {
           idleTime += Time.deltaTime;

           if(idleTime > timeUntilCrawling && stateInfo.normalizedTime % 1f < 0.02f)
           {
               boredAnimation = Random.Range(1, numberOfBoredAnimation +1);
           }
       }

       animator.SetFloat("Blend", boredAnimation, 0.2f, Time.deltaTime);
    }

    void ResetIdle()
    {
        isCrawling = false;
        idleTime = 0;
        boredAnimation = 0;
    }
}
