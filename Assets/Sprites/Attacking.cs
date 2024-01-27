using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Attacking : StateMachineBehaviour
{
    public enum bullet_spawn_function
    {
        STATIC,
        LINEAR_UP,
        LINEAR_DOWN,
        LINEAR_INWARD,
        LINEAR_OUTWARD,
        CUBIC_UP,
        CUBIC_DOWN,
        CUBIC_INWARD,
        CUBIC_OUTWARD,
        TOTAL_SPAWNS
    }
    public bullet_spawn_function function;

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
            //SpawnBullet(animator.transform.position);

            switch (function)
            {
                case bullet_spawn_function.STATIC:
                    SpawnBullet(animator.transform.position - new Vector3(animator.transform.localScale.x / 2.0f, 0.0f, 0.0f));
                    break;
                case bullet_spawn_function.LINEAR_UP:
                    UpdateSpawnPositionLinear(animator.transform, elapsed_time, attack_duration, true);
                    break;
                case bullet_spawn_function.LINEAR_DOWN:
                    UpdateSpawnPositionLinear(animator.transform, elapsed_time, attack_duration, false);
                    break;
                case bullet_spawn_function.LINEAR_INWARD:
                    UpdateSpawnPositionLinearInward(animator.transform, elapsed_time, attack_duration, true);
                    break;
                case bullet_spawn_function.LINEAR_OUTWARD:
                    UpdateSpawnPositionLinearInward(animator.transform, elapsed_time, attack_duration, false);
                    break;
                case bullet_spawn_function.CUBIC_UP:
                    UpdateSpawnPositionLinear(animator.transform, elapsed_time, attack_duration, true, true);
                    break;
                case bullet_spawn_function.CUBIC_DOWN:
                    UpdateSpawnPositionLinear(animator.transform, elapsed_time, attack_duration, false, true);
                    break;
                case bullet_spawn_function.CUBIC_INWARD:
                    UpdateSpawnPositionLinearInward(animator.transform, elapsed_time, attack_duration, true, true);
                    break;
                case bullet_spawn_function.CUBIC_OUTWARD:
                    UpdateSpawnPositionLinearInward(animator.transform, elapsed_time, attack_duration, false, true);
                    break;
            }


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

    void UpdateSpawnPositionLinear(Transform trans, float elapsed_time, float attack_duration, bool up, bool cubic = false)
    {
        int dir = up? 1 : -1;
        float t = elapsed_time / attack_duration;

        if(cubic)
            t = t < 0.5 ? 4 * t * t * t : 1 - (-2 * t + 2) * (-2 * t + 2) * (-2 * t + 2) / 2;

        Vector3 position = Vector3.Lerp(trans.position + trans.localScale / 2.0f * dir, trans.position - trans.localScale / 2.0f * dir, t);
        SpawnBullet(position);
    }
    void UpdateSpawnPositionLinearInward(Transform trans, float elapsed_time, float attack_duration, bool inward, bool cubic = false)
    {
        float t = elapsed_time / attack_duration;

        if (cubic)
            t = t < 0.5 ? 4 * t * t * t : 1 - (-2 * t + 2) * (-2 * t + 2) * (-2 * t + 2) / 2;

        if (inward)
        {
            Vector3 topposition = Vector3.Lerp(trans.position + trans.localScale / 2.0f, trans.position, t);
            Vector3 botposition = Vector3.Lerp(trans.position - trans.localScale / 2.0f, trans.position, t);
            SpawnBullet(topposition);
            SpawnBullet(botposition);
        }
        else
        {
            Vector3 topposition = Vector3.Lerp(trans.position, trans.position + trans.localScale / 2.0f, t);
            Vector3 botposition = Vector3.Lerp(trans.position, trans.position - trans.localScale / 2.0f, t);
            SpawnBullet(topposition);
            SpawnBullet(botposition);
        }
        
    }

    void SpawnBullet(Vector3 pos)
    {
        GameObject spawned_bulet = Instantiate(bulletPrefab);

        spawned_bulet.transform.SetPositionAndRotation(pos, Quaternion.identity);
    }
}
