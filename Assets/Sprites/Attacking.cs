using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.UIElements;

public class Attacking : StateMachineBehaviour
{
    List<GameObject> warningSignals;
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
        AGUJERO,
        AGUJERO_FAST,
        NINA,
        NINA_HIGH,
        TOTAL_SPAWNS
    }
    public bullet_spawn_function function;

    private float elapsed_time = 0.0f;
    public float attack_duration = 0.5f;
    public string myParameterName;
    public float y_offset_position = 0.0f;

    private float elapsed_bullet_time = 0.0f;

    public int frequence;
    public float Omega = 1.0f;
    public GameObject bulletPrefab;
    public GameObject bulletPrefabD;

    public GameObject warningSignal;

    private Vector3 bounds;
    public float min_scene_x;
    public float max_scene_x;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsed_time = 0.0f;
        bounds = new Vector3(animator.GetComponent<Transform>().localScale.x, animator.GetComponent<BossBehavior>().bulletBounds.x, 0.0f);
        switch(function)
        {
        case bullet_spawn_function.AGUJERO_FAST:
            StartSpawnPositionAgujeroFast(min_scene_x, max_scene_x);
            break;
        case bullet_spawn_function.NINA:
            StartSpawnPositionNina(min_scene_x, max_scene_x);
            break;
        case bullet_spawn_function.NINA_HIGH:
            StartSpawnPositionNina(min_scene_x, max_scene_x);
            break;
        }
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
                    UpdateSpawnPositionStatic(animator.transform, elapsed_time, attack_duration, 2);
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
                case bullet_spawn_function.AGUJERO:
                    UpdateSpawnPositionAgujero(min_scene_x, max_scene_x, elapsed_time, attack_duration);
                    break;
                case bullet_spawn_function.AGUJERO_FAST:
                    //UpdateSpawnPositionAgujeroFast(min_scene_x, max_scene_x, elapsed_time, attack_duration);
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
        if(function == bullet_spawn_function.AGUJERO_FAST)
            foreach(GameObject obj in warningSignals)
            {
                Destroy(obj);
            }
        if(function == bullet_spawn_function.NINA || function == bullet_spawn_function.NINA_HIGH)
        {
            Destroy(nina_hitbox);
        }
    }

    void UpdateSpawnPositionStatic(Transform trans, float elapsed_time, float attack_duration, int streams)
    {
        Vector3 top_position = trans.position + new Vector3(bounds.x, bounds.y, 0.0f);
        for(int i = 0; i < streams; i++) 
        {
            Vector3 position = top_position -  i * new Vector3(0.0f, (2.0f * bounds.y / (float)(streams - 1)), 0.0f);
            SpawnBullet(position);
        }
    }

    void UpdateSpawnPositionLinear(Transform trans, float elapsed_time, float attack_duration, bool up, bool cubic = false)
    {
        int dir = up? 1 : -1;
        float t = elapsed_time / attack_duration;

        if(cubic)
            t = t < 0.5 ? 4 * t * t * t : 1 - (-2 * t + 2) * (-2 * t + 2) * (-2 * t + 2) / 2;

        Vector3 position = Vector3.Lerp(trans.position + bounds * dir, trans.position - bounds * dir, t);
        SpawnBullet(position);
    }
    void UpdateSpawnPositionLinearInward(Transform trans, float elapsed_time, float attack_duration, bool inward, bool cubic = false)
    {
        float t = elapsed_time / attack_duration;

        if (cubic)
            t = t < 0.5 ? 4 * t * t * t : 1 - (-2 * t + 2) * (-2 * t + 2) * (-2 * t + 2) / 2;

        if (inward)
        {
            Vector3 topposition = Vector3.Lerp(trans.position + bounds, trans.position, t);
            Vector3 botposition = Vector3.Lerp(trans.position - bounds, trans.position, t);
            SpawnBullet(topposition);
            SpawnBullet(botposition);
        }
        else
        {
            Vector3 topposition = Vector3.Lerp(trans.position, trans.position + bounds, t);
            Vector3 botposition = Vector3.Lerp(trans.position, trans.position - bounds, t);
            SpawnBullet(topposition);
            SpawnBullet(botposition);
        }
        
    }

    void UpdateSpawnPositionAgujero(float min_x, float max_x, float elapsed_time, float attack_duration)
    {
        SpawnBulletDown(new Vector3(-10f, Mathf.Cos(elapsed_time / attack_duration) * 5.0f + 10.0f, 0.0f));
        SpawnBulletDown(new Vector3(7.0f, Mathf.Cos(1.0f-(elapsed_time / attack_duration)) * 5.0f + 10.0f, 0.0f));
    }

    void StartSpawnPositionAgujeroFast(float min_x, float max_x)
    {
        warningSignals = new List<GameObject>();
        float dt = (max_x - min_x)/frequence;
        for(int i = 0; i < frequence; ++i)
        {
            GameObject spawned_warning_signal = Instantiate(warningSignal);
            spawned_warning_signal.GetComponent<Transform>().position = new Vector3(min_x + dt * i, 2.0f, 0.0f);
            warningSignals.Add(spawned_warning_signal);
            for(int j = 0; j < 10; ++j)
            {
                SpawnBullet(new Vector3(min_x + dt * i, 24.0f + j * 5, 0.0f));
            }
        }

    }

    GameObject nina_hitbox;

    void StartSpawnPositionNina(float min_x, float max_x)
    {
        float dt = (max_x - min_x)/frequence;
        for(int i = 0; i < frequence; ++i)
        {
            Vector2 aim = new Vector2(min_x + dt * i, -9.0f);
            float y0 = -6.0f;
            float x0 = max_x + 3.0f;
            float alpha = Mathf.Atan((aim.y - y0 + (1.0f/2.0f)*9.8f * attack_duration* attack_duration) * (1.0f/(aim.x - x0)));
            float v = (aim.x - x0/(Mathf.Cos(alpha)*attack_duration));
            Vector2 final_vel = new Vector2();
            final_vel.x = Mathf.Cos(alpha) * v;
            final_vel.y = Mathf.Sin(alpha) * v;

            GameObject spawned_bullet = Instantiate(bulletPrefab);
            spawned_bullet.GetComponent<Transform>().position = new Vector3(x0, y0, 0.0f);
            spawned_bullet.GetComponent<Rigidbody2D>().velocity = final_vel;
        }

        nina_hitbox = Instantiate(bulletPrefabD);
        nina_hitbox.GetComponent<Transform>().position = new Vector3(max_x + 2.0f, -10.0f, 0.0f);
    }
    void SpawnBullet(Vector3 pos)
    {
        GameObject spawned_bulet = Instantiate(bulletPrefab);

        spawned_bulet.transform.SetPositionAndRotation(new Vector3(pos.x, pos.y + y_offset_position, 0.0f), Quaternion.identity);
    }

    void SpawnBulletDown(Vector3 pos)
    {
        GameObject spawned_bullet = Instantiate(bulletPrefabD);
        spawned_bullet.transform.SetPositionAndRotation(pos, Quaternion.identity);
        spawned_bullet.GetComponent<BasicProjectileBehaviour>().mAmplitude = max_scene_x - min_scene_x;
        spawned_bullet.GetComponent<BasicProjectileBehaviour>().mOmega = Omega;
    }
}
