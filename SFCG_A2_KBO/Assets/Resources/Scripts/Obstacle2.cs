using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle2 : MonoBehaviour
{
    public float speed;
    public Transform[] moveSpots;
    private int randomSpots;
    private float waitTime;
    public float StartWaitTime;

    private void Start()
    {
        waitTime = StartWaitTime;
        randomSpots = Random.Range(0, moveSpots.Length);
    }
    private void Update()
    {



        transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpots].position, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, moveSpots[randomSpots].position) < 0.2f)
        {

            if(waitTime <= 0)
            {
                
                randomSpots = Random.Range(0, moveSpots.Length);
                waitTime = StartWaitTime;
            }
            else
            {
                //if waitTime = 3 then red square will stand idle for 3 secs
                waitTime -= Time.deltaTime;
            }
        }
    }

  
}
