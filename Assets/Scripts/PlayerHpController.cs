using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpController : MonoBehaviour
{
    public Image healthBar;
    public float BaseHealth = 100.0f;
    public float DamageoOfHit = 20.00f;
    public float HealthAmount = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Hitreceived(DamageoOfHit);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ResetHealth();
        }
    }

    public void Hitreceived(float dmg)
    {
        HealthAmount -= dmg;
        healthBar.fillAmount = HealthAmount / 100.0f;
    }

    public void ResetHealth()
    {
        HealthAmount = BaseHealth;
        healthBar.fillAmount = HealthAmount;
    }
}
