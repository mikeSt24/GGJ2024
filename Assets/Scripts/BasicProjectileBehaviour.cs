using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Timeline;
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

    public float  BulletSpeed = 10.0f;
    public Direction BulletDirection = Direction.Left;
    public PathFunction PathType;

    private Vector3 mInitPos = Vector3.zero;
    private int mDir = -1;
    private float mAmplitude = 2.0f;


    void Start()
    {
        mInitPos = transform.position;
        switch(BulletDirection)
        {
            case Direction.Right: case Direction.Upward:
            mDir = 1;
            break;
            default:
            case Direction.Left: case Direction.Downward:
                mDir = -1;
            break;
        }
        if(BulletSpeed < 0.0f)
        {
            BulletSpeed *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {

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
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet") == true) return;
        Debug.Log("I'm dead");
        Destroy(gameObject);
    }

    void LinearPath()
    {
        Vector3 new_pos = transform.position;

        if(BulletDirection == Direction.Left || BulletDirection == Direction.Right)
        {
            new_pos.x += Time.deltaTime * BulletSpeed * mDir;
        }
        else
        {
            new_pos.y += Time.deltaTime * BulletSpeed * mDir;
        }

        transform.SetPositionAndRotation(new_pos, transform.rotation);
    }
    void SinPath()
    {
        Vector3 new_pos = transform.position;


        if (BulletDirection == Direction.Left || BulletDirection == Direction.Right)
        {
            new_pos.x += Time.deltaTime * BulletSpeed * mDir;
            new_pos.y = mInitPos.y + mAmplitude * Mathf.Sin(new_pos.x - mInitPos.x);
        }
        else
        {
            new_pos.y += Time.deltaTime * BulletSpeed * mDir;
            new_pos.x = mInitPos.x + mAmplitude * Mathf.Sin(new_pos.y - mInitPos.y);
        }


        transform.SetPositionAndRotation(new_pos, transform.rotation);
    }
    void CosPath()
    {
        Vector3 new_pos = transform.position;


        if (BulletDirection == Direction.Left || BulletDirection == Direction.Right)
        {
            new_pos.x += Time.deltaTime * BulletSpeed * mDir;
            new_pos.y = mInitPos.y + mAmplitude * Mathf.Cos(new_pos.x - mInitPos.x);
        }
        else
        {
            new_pos.y += Time.deltaTime * BulletSpeed * mDir;
            new_pos.x = mInitPos.x + mAmplitude * Mathf.Cos(new_pos.y - mInitPos.y);
        }

        transform.SetPositionAndRotation(new_pos, transform.rotation);
    }
}
