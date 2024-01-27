using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHpController : MonoBehaviour
{
    //public GameObject healthBar_prefab;
    public GameObject heart_prefab;
    public int hearts = 3;

    public GameObject healthBar;
    private float height = 350;
    private float start = -800;
    public float Heart_Step = 150.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        //healthBar = Instantiate(healthBar_prefab);

        int count = healthBar.transform.childCount;
        for(int i = 0; i < hearts - count; i++)
        {
            GameObject obj = Instantiate(heart_prefab,healthBar.transform);
            obj.transform.localPosition = new Vector3(start, height, 0.0f) + new Vector3(i * Heart_Step, 0.0f,0.0f);
        }
    }
    public void Hitreceived()
    {
        --hearts;
        Destroy(transform.GetChild(healthBar.transform.childCount - 1).gameObject);
        if(hearts == 0)
        {
            SceneManager.LoadScene("LoseScreen");
        }
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
            Debug.Log("O nooooooo");
            Hitreceived();
        }
    }
}
