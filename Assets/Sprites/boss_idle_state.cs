using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Random;

public class boss_idle_state : StateMachineBehaviour
{
    private float elapsed_time = 0.0f;
    public float attack_cooldown = 0.5f;
    private attack_info previous_attack = null;

    public enum boss_attacks
    {
        LINE_ATTACK,
        SIN_ATTACK,
        TOTAL_ATTACKS
    }

    [System.Serializable]
    public class attack_info
    {
        public boss_attacks attack;
        public float probability;
        public float cooldown;
        public string name;
    }

    public List<attack_info> attacks;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsed_time = 0.0f;
        if(previous_attack != null) 
        {
            attack_cooldown = previous_attack.cooldown;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsed_time += Time.deltaTime;

        if(elapsed_time >= attack_cooldown) 
        {
            float mynum = UnityEngine.Random.Range(0.0f, 1.0f);

            float accumulated_chance = 0.0f;

            bool found = false;

            for(boss_attacks i = boss_attacks.LINE_ATTACK; i < boss_attacks.TOTAL_ATTACKS; i++) 
            {
                for(int j = 0; j < attacks.Capacity;j++) // more of a safety meassure than anything else
                {
                    if (attacks[j].attack == i)
                    {
                        accumulated_chance += attacks[j].probability;
                        if(accumulated_chance >= mynum)
                        {
                            animator.SetBool(attacks[j].name, true);
                            previous_attack = attacks[j];
                            found = true;
                        }
                        break;
                    }
                }
                if (found) { break; }
            }

            elapsed_time = 0.0f;
        }
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
