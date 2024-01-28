using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class face_velocity_dir : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * (angle) + 90.0f);
    }
}
