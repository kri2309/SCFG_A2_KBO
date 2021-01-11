using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
using UnityEngine.UI;

public class snakeheadController : MonoBehaviour
{
    snakeGenerator mysnakegenerator;
    foodGenerator fg;
    GridGraph gg;
    GameObject GameOver;
    GameObject Enemy;
    public string NextLevel;
    bool dead = false; 
   
    

    private void Start()
    {
        fg = Camera.main.GetComponent<foodGenerator>();
        mysnakegenerator = Camera.main.GetComponent<snakeGenerator>();
        gg = FindObjectOfType<AstarPath>().data.gridGraph;

        if ( NextLevel == "Ending")
        {
            mysnakegenerator.snakelength = PlayerPrefs.GetInt("snakelength");
        }

        dead = false; 
    }

    private void FixedUpdate() //calling less than update maybe idk
    {
        mysnakegenerator.ScanGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead) {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.position -= new Vector3(1f, 0);
                fg.eatFood(this.transform.position, mysnakegenerator);
                CheckFinish();

                //mysnakegenerator.hitTail(transform.position, mysnakegenerator.snakelength);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1f, 0);
                fg.eatFood(this.transform.position, mysnakegenerator);
                CheckFinish();

                //mysnakegenerator.hitTail(transform.position, mysnakegenerator.snakelength);

            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.position += new Vector3(0, 1f);
                fg.eatFood(this.transform.position, mysnakegenerator);
                CheckFinish();

                // mysnakegenerator.hitTail(transform.position, mysnakegenerator.snakelength);

            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                transform.position -= new Vector3(0, 1f);
                fg.eatFood(this.transform.position, mysnakegenerator);
                CheckFinish();

                //mysnakegenerator.hitTail(transform.position, mysnakegenerator.snakelength);

            }

            CheckHit();
            CheckObstacle();
        }

    }


    void CheckFinish()
    {
        
        
        if (mysnakegenerator.snakelength >= 6 && transform.position== new Vector3 (19,2))
        {

            FindObjectOfType<timerManager>().timerStarted = false;
            PlayerPrefs.SetInt("snakelength", mysnakegenerator.snakelength);
            SceneManager.LoadScene(NextLevel);

        }

        

    }

    void CheckObstacle()
    {
      
      


        if (!gg.GetNode((int)transform.position.x, (int)transform.position.y).Walkable || mysnakegenerator.hitTail(transform.position, mysnakegenerator.snakelength))
        {
            dead = true;
            mysnakegenerator.Death();
            
        }
        
    }
    /*
    void GameLost()
    {
        GameOver = Instantiate(Resources.Load<GameObject>("Prefabs/ButtonPrefab"), new Vector3(0f, 0f), Quaternion.identity);

        GameOver.GetComponentInChildren<Text>().text = "You Lost!";

        GameOver.GetComponentInChildren<Button>().onClick.AddListener(
                () =>
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });
    }
    */
    void CheckHit()
    {
        if (GameObject.Find("Enemy Robot"))
        {
            Enemy = GameObject.Find("Enemy Robot");
            snakeGenerator sgEnemy = Enemy.GetComponent<snakeGenerator>();

            if (sgEnemy.hitTail(transform.position, sgEnemy.snakelength) || Enemy.transform.position == transform.position)
            {
                dead = true;
                mysnakegenerator.Death();
            }
        }
        if(mysnakegenerator.hitTail(transform.position, mysnakegenerator.snakelength))
        {
            dead = true;
            mysnakegenerator.Death();
        }
        
    }
}
