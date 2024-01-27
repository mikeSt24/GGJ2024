using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public Vector2 bulletBounds;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("player_bullet"))
        {
            gameObject.GetComponent<BossHpController>().Hitreceived(0.1f); // Damage Temp
        }
    }
}
