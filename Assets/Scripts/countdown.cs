using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class countdown : MonoBehaviour
{
    public Animator player_brain;
    public Animator boss_brain;
    public float time = 3;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        text.SetText((Mathf.Ceil(time)).ToString());
        if(Mathf.Ceil(time) == 1)
        {
            text.SetText("l");
        }
        if(Mathf.Ceil(time) == 0)
        {
            text.SetText("GO!");
        }
        if(time <= -1.0f)
        {
            player_brain.SetBool("Countdown", false);
            boss_brain.SetBool("Countdown", false);
            Destroy(gameObject);
        }

    }
}
