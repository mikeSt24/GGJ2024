using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossHpController: MonoBehaviour
{
    //public GameObject healthBar_Prefab;
    public Image healthBar;

    public float BaseHealth = 100.0f;
    public float DamageoOfHit = 20.00f;
    public float HealthAmount = 100.0f;
    // Start is called before the first frame update
    private float TimeRed = 0.1f;
    private float currentTime = 0;
    private bool hit = false;

    //private void Start()
    //{
    //    healthBar_Prefab = Instantiate(healthBar_Prefab);
    //    healthBar = healthBar_Prefab.transform.GetChild(2).GetComponent<Image>();
    //}
    private void Update()
    {
        if(hit)
        {
            currentTime += Time.deltaTime;
        }
        if(currentTime > TimeRed)
        {
            hit = false;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    public void Hitreceived(float dmg)
    {
        currentTime = 0;
        hit = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        HealthAmount -= dmg;
        healthBar.fillAmount = HealthAmount / 100.0f;
        if(HealthAmount <= 0.0f)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

    public void ResetHealth()
    {
        HealthAmount = BaseHealth;
        healthBar.fillAmount = HealthAmount;
    }
}
