using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_bullet : MonoBehaviour
{
    Vector2 direction;
    public float speed;
    Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tr.position = new Vector2(tr.position.x + direction.x * speed, tr.position.y + direction.y * speed);
    }
}
