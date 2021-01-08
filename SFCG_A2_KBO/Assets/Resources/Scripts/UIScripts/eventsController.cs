using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class eventsController : MonoBehaviour
{
    
    public GameObject userInputField,textDisplay;
    public string theName;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("User"))
        {
            GameObject.Find("User").GetComponent<Text>().text = "User: " + PlayerPrefs.GetString("name");
        }

        if (GameObject.Find("Time") && FindObjectOfType<timerManager>())
        {
            GameObject.Find("Time").GetComponent<Text>().text = "Time taken: " + FindObjectOfType<timerManager>().GetTime();
        }

        if (GameObject.Find("Length"))
        {
            GameObject.Find("Length").GetComponent<Text>().text = "Snake length: " + PlayerPrefs.GetInt("snakelength").ToString();
        }


    }
    public void StoreName()
    {
        theName = userInputField.GetComponent<Text>().text;
        textDisplay.GetComponent<Text>().text = "Welcome " + theName;
        PlayerPrefs.SetString("name", theName);
        StartCoroutine(StartGame());

    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Level 1");

    }

    

}
