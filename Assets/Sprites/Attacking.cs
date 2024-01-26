using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : StateMachineBehaviour
{
    private float elapsed_time = 0.0f;
    public float attack_duration = 0.5f;
    public string myParameterName;

    private float elapsed_bullet_time = 0.0f;

    public int frequence;
    public GameObject bulletPrefab;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsed_time = 0.0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsed_time += Time.deltaTime;
        elapsed_bullet_time += Time.deltaTime;

        if(elapsed_bullet_time >= (1.0f / (float)frequence))
        {
            SpawnBullet();
            elapsed_bullet_time = 0.0f;
        }


        if (elapsed_time >= attack_duration)
        {
            animator.SetBool(myParameterName, false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    void SpawnBullet()
    {
        Debug.Log("Bullet spawned");
    }
}
