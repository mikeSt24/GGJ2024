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
    public Image face;

    public float BaseHealth = 0.0f;
    public float DamageoOfHit = 20.00f;
    public float HealthAmount = 0.0f;
    // Start is called before the first frame update
    private float TimeRed = 0.1f;
    private float currentTime = 0;
    private bool hit = false;
    private Vector3 mInitPos = Vector3.zero;
    private float width = 0.0f;

    private void Start()
    {
        //healthBar_Prefab = Instantiate(healthBar_Prefab);
        //healthBar = healthBar_Prefab.transform.GetChild(2).GetComponent<Image>();
        mInitPos = face.transform.position;
        width = healthBar.GetComponent<RectTransform>().rect.width;
        face.gameObject.transform.position = new Vector3(mInitPos.x, face.gameObject.transform.position.y, face.gameObject.transform.position.z);
    }
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

        HealthAmount += dmg * 2.0f;
        healthBar.fillAmount = HealthAmount / 100.0f;

        //Debug.Log(healthBar.GetComponent<RectTransform>().rect.width * healthBar.fillAmount);
        //
        //float x = Mathf.Lerp(mInitPos.x, mInitPos.x + width, healthBar.fillAmount);
        //Debug.Log(x);
        //
        //face.gameObject.transform.position = new Vector3(x, face.gameObject.transform.position.y, face.gameObject.transform.position.z);

        if (HealthAmount >= 100.0f)
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
