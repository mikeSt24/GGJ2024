using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButoonBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }


    public void ActivatePauseMenu()
    {
        Time.timeScale = 0.0f;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        gameObject.GetComponent<Button>().enabled = false;
        gameObject.GetComponent<Image>().enabled = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        transform.GetComponent<Button>().enabled = true;
        transform.GetComponent<Image>().enabled = true;
    }
    public void OptionsButton()
    {
        GameObject OptionsBut = gameObject;
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).name == "OPTIONS")
            {
                OptionsBut = transform.GetChild(i).gameObject;
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        OptionsBut.transform.GetComponent<Button>().enabled = false;
        OptionsBut.transform.GetComponent<Image>().enabled = false;

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
                    if(transform.GetChild(i).transform.GetChild(j).gameObject.name != "Text (TMP)")
                    {
                        transform.GetChild(i).transform.GetChild(j).gameObject.SetActive(false);
                    }
                }

                transform.GetChild(i).transform.GetComponent<Button>().enabled = true;
                transform.GetChild(i).transform.GetComponent<Image>().enabled = true;
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
