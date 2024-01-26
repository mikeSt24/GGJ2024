using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    public Rigidbody2D rb; 
    public float mSpeed;
    public float mMaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //For now this will be hardcoded, this could change
        rb.AddForce(Input.GetAxis("Horizontal") * mSpeed * new Vector2(1,0));
        //Force for a maximum speed
        if(Mathf.Abs(rb.velocity.x) > mMaxSpeed)
        {
            if(rb.velocity.x > 0)
                rb.velocity = new Vector2(mMaxSpeed, rb.velocity.y);
            else
                rb.velocity = new Vector2(-mMaxSpeed, rb.velocity.y);
        }
        
    }
}
