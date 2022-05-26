using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    [SerializeField] float timeUntilBoredMin;
    [SerializeField] float timeUntilBoredMax;
    int numberOfBoredAnimation = 1;
    public float timeUntilBored;
    bool isBored;
    float idleTime;
    int boredAnimation;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       ResetIdle(); 
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(isBored == false) 
       {
           idleTime += Time.deltaTime;

           if(idleTime > timeUntilBored && stateInfo.normalizedTime % 1f < 0.02f)
           {
               isBored = true;
               boredAnimation = Random.Range(1, numberOfBoredAnimation +1);
           }
       }
       else if(stateInfo.normalizedTime % 1f > 0.98f)
       {
           ResetIdle();    
       }

       animator.SetFloat("Blend", boredAnimation, 0.5f, Time.deltaTime);
    }

    void ResetIdle()
    {
        isBored = false;
        idleTime = 0;
        boredAnimation = 0;
        timeUntilBored = Random.Range(timeUntilBoredMin, timeUntilBoredMax);
    }

}
