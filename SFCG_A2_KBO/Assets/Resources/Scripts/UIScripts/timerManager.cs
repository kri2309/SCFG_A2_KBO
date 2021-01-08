using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerManager : MonoBehaviour
{

    public bool timerStarted;

    int timerValue=0;

    public Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(timer());
    }

    IEnumerator timer()
    {
        while(true)
        { 
            if (timerStarted)
            {
                //measure the time
                timerValue++;

                int minutes = timerValue / 60;
                int seconds = timerValue % 60;

                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);


                yield return new WaitForSeconds(1f);
            }
            else
            {
                timerText.text = "";
                yield return null;

            }
            
        }
    }

    public string GetTime()
    {
        int minutes = timerValue / 60;
        int seconds = timerValue % 60;

        return string.Format("{0:00}:{1:00}", minutes, seconds);

    }
 

    
}
