using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButoonBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator player_anim;

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
        if(player_anim.GetBool("Countdown"))
        {
            return;
        }

        Time.timeScale = 0.0f;
        for (int i = 0; i < transform.childCount; i++)
        {
            //if(transform.GetChild(i).name == "PauseImages")
            //{
            //    for (int j = 0; j < transform.GetChild(i).transform.childCount; j++)
            //    {
            //        transform.GetChild(i).transform.GetChild(j).gameObject.SetActive(true);
            //    }
            //}
            transform.GetChild(i).gameObject.SetActive(true);
        }

        //gameObject.GetComponent<Button>().enabled = false;
        //gameObject.GetComponent<Image>().enabled = false;
    }

    public void ResumeGame()
    {

        Debug.Log("Goo");
        Time.timeScale = 1.0f;
        for (int i = 0; i < transform.childCount; i++)
        {
            //if (transform.GetChild(i).name == "PauseImages")
            //{
            //    for (int j = 0; j < transform.GetChild(i).transform.childCount; j++)
            //    {
            //        transform.GetChild(i).transform.GetChild(j).gameObject.SetActive(false);
            //    }
            //}
            transform.GetChild(i).gameObject.SetActive(false);
        }

        //transform.GetComponent<Button>().enabled = true;
        //transform.GetComponent<Image>().enabled = true;
    }
    public void OptionsButton()
    {
        Debug.Log("1");

        GameObject OptionsBut = gameObject;
        for (int i = 0; i < transform.childCount; i++)
        {
            //if (transform.GetChild(i).name == "PauseImages")
            //{
            //    for (int j = 0; j < transform.GetChild(i).transform.childCount; j++)
            //    {
            //        //if (OptionsBut.transform.GetChild(i).gameObject.name != "Text (TMP)")
            //        //{
            //        //    OptionsBut.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = false;
            //        //}
            //        OptionsBut.transform.GetChild(i).gameObject.SetActive(false);
            //    }
            //}
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

        Debug.Log("2");
    }
    public void ExitOptionsButton()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //if (transform.GetChild(i).name == "PauseImages")
            //{
            //    for (int j = 0; j < transform.GetChild(i).transform.childCount; j++)
            //    {
            //        transform.GetChild(i).transform.GetChild(j).gameObject.SetActive(true);
            //    }
            //}
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
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
