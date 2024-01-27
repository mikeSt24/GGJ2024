using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class player_shooting : MonoBehaviour
{
    public GameObject bullet;
    public float bullet_speed;
    public float bullets_per_second;
    float t;
    Transform tr;
    Rigidbody2D rb;
    UnityEngine.Vector2 dir;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        t = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.x  > 0)
        {
            dir = new UnityEngine.Vector2(1.0f, 0.0f);
        }
        if(rb.velocity.x < 0)
        {
            dir = new UnityEngine.Vector2(-1.0f, 0.0f);
        }

        t -= Time.deltaTime;

        if(Input.GetKey(KeyCode.X) && t <= 0)
        {
            GameObject local_bullet = Instantiate(bullet, new UnityEngine.Vector3(dir.x, dir.y, 0) + tr.position, new quaternion());
            local_bullet.GetComponent<player_bullet>().direction = dir;
            local_bullet.GetComponent<player_bullet>().speed = bullet_speed;
            t = 1.0f / bullets_per_second;
        }
        
    }
}
