using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_jumpup_brain : StateMachineBehaviour
{

    Transform tr;
    Rigidbody2D rb;
    float initial_pos;
    public float jmp_height;

    bool stop_jump = false;

    float t;
    public float max_t = 0.3f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        tr = animator.GetComponent<Transform>();
        rb = animator.GetComponent<Rigidbody2D>();
        initial_pos = tr.position.y;
        t = 0.0f;
        stop_jump = false;
    }

    float EasingFunc(float t)
    {
        return Mathf.Sin(Mathf.PI * t / 2);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        t += Time.deltaTime;
        if(!stop_jump)
            tr.position = new Vector2(tr.position.x, initial_pos + jmp_height * EasingFunc(t/max_t));
        if(!Input.GetKey(KeyCode.Z)  && t > 0.1f) //Trigger jump
        {
            animator.SetBool("JumpUp", false);
            animator.SetBool("JumpDown", true);
            stop_jump = true;
            return;
            
        }
        if(t >= max_t)
        {
            animator.SetBool("JumpUp", false);
            animator.SetBool("JumpDown", true);
            stop_jump = true;
            return;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
