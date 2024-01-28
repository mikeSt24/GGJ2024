using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButoonBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator player_anim;
    private bool active = false;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivatePauseMenu();
        }
    }


    public void ActivatePauseMenu()
    {
        if(player_anim.GetBool("Countdown") || active)
        {
            return;
        }
        active = true;
        Time.timeScale = 0.0f;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

    }

    public void ResumeGame()
    {

        active = false;
        Time.timeScale = 1.0f;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

    }
    public void OptionsButton()
    {

        GameObject OptionsBut = gameObject;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "OPTIONS")
            {
                OptionsBut = transform.GetChild(i).gameObject;
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        OptionsBut.GetComponent<Button>().enabled = false;
        for (int i = 0; i < OptionsBut.transform.childCount; i++)
        {
            OptionsBut.transform.GetChild(i).gameObject.SetActive(true);
        }

    }
    public void ExitOptionsButton()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "OPTIONS")
            {
                for (int j = 0; j < transform.GetChild(i).transform.childCount; j++)
                {
                    transform.GetChild(i).transform.GetChild(j).gameObject.SetActive(false);
                }
                transform.GetChild(i).GetComponent<Button>().enabled = true;
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }


    }
    public void ExitGame()
    {
        active = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
