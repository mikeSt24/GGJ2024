using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BasicProjectileBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public enum PathFunction
    {
        linear,
        sin_fun,
        cos_fun
    }
    public enum Direction
    {
        Left,
        Right,
        Downward,
        Upward
    }

    public float BulletSpeed = 10.0f;
    public Direction BulletDirection = Direction.Left;
    public PathFunction PathType;

    private Vector3 mInitPos = Vector3.zero;
    private int mDir = -1;
    private float t = 0.0f;

    public bool mCustomVector = false;
    public Vector3 mVecDir = Vector3.zero;

    Animator mAnimator;

    public float mAmplitude = 2.0f;

    public float mOmega = 0.01f;


    void Start()
    {
        mInitPos = transform.position;
        mVecDir.z = 0.0f;
        if(mCustomVector && mVecDir.x == 0.0f)
        {
            mCustomVector = false;
            if (mVecDir.y >= 0.0f)
            {
                BulletDirection = Direction.Upward;
            }
            else
            {
                BulletDirection = Direction.Downward;

            }
        }



        switch (BulletDirection)
        {
            case Direction.Right: case Direction.Upward:
                mDir = 1;
                break;
            default:
            case Direction.Left: case Direction.Downward:
                mDir = -1;
                break;
        }
        if (BulletSpeed < 0.0f)
        {
            BulletSpeed *= -1;
        }

        mAnimator = GetComponent<Animator>();  
    }

    // Update is called once per frame
    void Update()
    {
        mPrevPos = transform.position;
        switch (PathType)
        {
            case PathFunction.sin_fun:
                SinPath();
                break;
            case PathFunction.cos_fun:
                CosPath();
                break;

            default:
            case PathFunction.linear:
                LinearPath();
                break;
        }
        t += Time.deltaTime * BulletSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("bullet") == true || collision.CompareTag("player_bullet") || collision.CompareTag("boss")) return;
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player got hit");
            collision.gameObject.GetComponent<PlayerHpController>().Hitreceived();
        }
        if(mAnimator != null) 
        {
            mAnimator.SetBool("dead", true);
            this.enabled = false;
            return;
        }

        Destroy(gameObject);
    }

    public void SetVectorDirector(Vector3 custm_dir)
    {
        mVecDir = custm_dir;
        mCustomVector = true;
    }

    private void LinearPath()
    {
        Vector3 new_pos = transform.position;
        Quaternion rot = transform.rotation;

        if (mCustomVector == false)
        {
            if (BulletDirection == Direction.Left || BulletDirection == Direction.Right)
            {
                new_pos.x += Time.deltaTime * BulletSpeed * mDir;


                rot = Quaternion.Euler(0.0F, 0.0F, Mathf.Asin(mDir)*180.0f/Mathf.PI);
            }
            else
            {
                new_pos.y += Time.deltaTime * BulletSpeed * mDir;
            }
        }
        else
        {
            rot = Quaternion.Euler(0.0F, 0.0F, (Mathf.Atan2(mVecDir.normalized.y, mVecDir.normalized.x) + Mathf.PI / 2.0f) * 180.0f / Mathf.PI);

            new_pos = mInitPos + mVecDir * t;

        }


        transform.SetPositionAndRotation(new_pos, rot);
    }

    private Vector3 mPrevPos = Vector3.zero;

    private void SinPath()
    {
        Vector3 new_pos = transform.position;
        Quaternion rot = transform.rotation;

        if (mCustomVector == false)
        {
            if (BulletDirection == Direction.Left || BulletDirection == Direction.Right)
            {
                new_pos.x += Time.deltaTime * BulletSpeed * mDir;
                new_pos.y = mInitPos.y + mAmplitude * Mathf.Sin(t * mOmega);
            }
            else
            {
                new_pos.y += Time.deltaTime * BulletSpeed * mDir;
                new_pos.x = mInitPos.x + mAmplitude * Mathf.Sin(t * mOmega);
            }
        }
        else
        {
            new_pos.x = mInitPos.x + t;
            new_pos.y = mInitPos.y + mAmplitude * Mathf.Sin(t * mOmega) + mVecDir.y*t/ mVecDir.x;
        }

        Vector3 dir_norm = new_pos - mPrevPos;dir_norm = dir_norm.normalized;
        rot = Quaternion.Euler(0.0F, 0.0F, (Mathf.Atan2(dir_norm.y, dir_norm.x) + Mathf.PI / 2.0f) * 180.0f / Mathf.PI);
        //rot = Quaternion.LookRotation(Vector3.RotateTowards(transform.position, new_pos, Time.deltaTime, 0.0f));




        transform.SetPositionAndRotation(new_pos, rot);
    }
    private void CosPath()
    {
        Vector3 new_pos = transform.position;
        Quaternion rot = transform.rotation;

        if (mCustomVector == false)
        {
            if (BulletDirection == Direction.Left || BulletDirection == Direction.Right)
            {
                new_pos.x += Time.deltaTime * BulletSpeed * mDir;
                new_pos.y = mInitPos.y + mAmplitude * Mathf.Cos(t * mOmega);
            }
            else
            {
                new_pos.y += Time.deltaTime * BulletSpeed * mDir;
                new_pos.x = mInitPos.x + mAmplitude * Mathf.Cos(t * mOmega);
            }
        }
        else
        {
            new_pos.x = mInitPos.x + t;
            new_pos.y = mInitPos.y + mAmplitude * Mathf.Cos(t * mOmega) + mVecDir.y * t / mVecDir.x;
        }

        Vector3 dir_norm = new_pos - mPrevPos; dir_norm = dir_norm.normalized;
        rot = Quaternion.Euler(0.0F, 0.0F, (Mathf.Atan2(dir_norm.y, dir_norm.x) + Mathf.PI / 2.0f) * 180.0f / Mathf.PI);
        //rot = Quaternion.LookRotation(Vector3.RotateTowards(transform.position, new_pos, Time.deltaTime, 0.0f));
        transform.SetPositionAndRotation(new_pos, rot);
    }

}
