using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHpController : MonoBehaviour
{
    //public GameObject healthBar_prefab;
    public GameObject heart_prefab;
    public int hearts = 3;

    public GameObject healthBar;
    private float height = 480;
    private float start = -800;
    public float Heart_Step = 150.0f;
    private bool CanTakeDamage = true;
    private float InvencibleFor = 3.0f;
    private float timer = 0.0f;
    private bool blincking = true;
    private float blinckTime = 0.1f;
    private bool ghoost = false;
    private float blicktimer = 0.0f;

    public Animator brain;

    // Start is called before the first frame update
    void Start()
    {
        //healthBar = Instantiate(healthBar_prefab);
        brain = GetComponent<Animator>();

        int count = healthBar.transform.childCount;
        for(int i = 0; i < hearts - count; i++)
        {
            GameObject obj = Instantiate(heart_prefab,healthBar.transform);
            obj.transform.localPosition = new Vector3(start, height, 0.0f) + new Vector3(i * Heart_Step, 0.0f,0.0f);
        }
    }
    private void Update()
    {
        if(!CanTakeDamage)
        {
            if(timer >= InvencibleFor)
            {
                CanTakeDamage = true;
                GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                return;
            }
            if(blincking)
            {
                if (blicktimer >= blinckTime)
                {
                    ghoost = !ghoost;
                    blicktimer = 0.0f;
                }
                if (ghoost)
                {
                    GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
                else
                {
                    GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                }
            }
            
        }
        timer += Time.deltaTime;
        blicktimer += Time.deltaTime;
    }
    public void Hitreceived()
    {
        if(!CanTakeDamage)
        {
            return;
        }
        --hearts;

        Destroy(healthBar.transform.GetChild(healthBar.transform.childCount - 1).gameObject);
        if(hearts == 0)
        {
            brain.SetTrigger("Die");
        }
        MakeInvulnerable();
    }

    public void ResetHealth()
    {
        ++hearts;
        GameObject obj = Instantiate(heart_prefab, healthBar.transform);
        obj.transform.position = new Vector3(start, height, 0.0f) + new Vector3(healthBar.transform.childCount * Heart_Step, 0.0f, 0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("bullet"))
        {
            Hitreceived();
        }
    }

    public void MakeInvulnerable(float untouchable = 3.0f,bool blk = true)
    {
        CanTakeDamage = false;

        ghoost = false;
        timer = 0.0f;
        blicktimer = 0.0f;
        blincking = blk;
        InvencibleFor = untouchable;
    }
}
