using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_bullet : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    Transform tr;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.position = new Vector2(tr.position.x + direction.x * speed * Time.deltaTime, tr.position.y + direction.y * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        animator.SetBool("dead", true);
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
    }
}
