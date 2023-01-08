using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    [SerializeField] float timeUntilBoredMin;
    [SerializeField] float timeUntilBoredMax;
    int numberOfBoredAnimation = 2;
    public float timeUntilBored;
    bool isBored;
    float idleTime;
    int boredAnimation;
    int previousBoredAnimation;  
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        ResetIdle(); 
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("die") == true) {return;}

        if(isBored == false) 
        {
            idleTime += Time.deltaTime;

            if(idleTime > timeUntilBored && stateInfo.normalizedTime % 1f < 0.02f)
            {
                isBored = true;
                previousBoredAnimation = boredAnimation;
                boredAnimation = Random.Range(1, numberOfBoredAnimation + 1);
                boredAnimation = boredAnimation * 2 - 1;

                animator.SetFloat("Blend", boredAnimation - 1);
            }
        }
        else if(stateInfo.normalizedTime % 1f > 0.98f)
        {
            ResetIdle();    
        }

        if (previousBoredAnimation != boredAnimation)
            animator.SetFloat("Blend", boredAnimation, 0.2f, Time.deltaTime);
    }

    void ResetIdle()
    {
        if(isBored)
        {
            previousBoredAnimation = boredAnimation;
            boredAnimation--;
        }
        isBored = false;
        idleTime = 0;
        timeUntilBored = Random.Range(timeUntilBoredMin, timeUntilBoredMax);
    }

}
