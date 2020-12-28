﻿using System.Collections;
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

    private void Start()
    {
        fg = Camera.main.GetComponent<foodGenerator>();
        mysnakegenerator = Camera.main.GetComponent<snakeGenerator>();
        gg = FindObjectOfType<AstarPath>().data.gridGraph;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(1f,0);
            fg.eatFood(this.transform.position);
            CheckFinish();
            CheckObstacle();
            mysnakegenerator.hitTail(transform.position, mysnakegenerator.snakelength);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1f, 0);
            fg.eatFood(this.transform.position);
            CheckFinish();
            CheckObstacle();
            mysnakegenerator.hitTail(transform.position, mysnakegenerator.snakelength);

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 1f);
            fg.eatFood(this.transform.position);
            CheckFinish();
            CheckObstacle();
            mysnakegenerator.hitTail(transform.position, mysnakegenerator.snakelength);

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0, 1f);
            fg.eatFood(this.transform.position);
            CheckFinish();
            CheckObstacle();
            mysnakegenerator.hitTail(transform.position, mysnakegenerator.snakelength);

        }

       // Debug.Log(mysnakegenerator.hitTail(this.transform.position, mysnakegenerator.snakelength)); 
    }


    void CheckFinish()
    {

        if (mysnakegenerator.snakelength >= 6 && transform.position== new Vector3 (19,2))
        {

            SceneManager.LoadScene("Level 2");
        }

    }

    void CheckObstacle()
    {
        
        if (!gg.GetNode((int)transform.position.x, (int)transform.position.y).Walkable)
        {
            GameOver = Instantiate(Resources.Load<GameObject>("Prefabs/ButtonPrefab"), new Vector3(0f, 0f), Quaternion.identity);

            GameOver.GetComponentInChildren<Text>().text = "You Lost!";

            GameOver.GetComponentInChildren<Button>().onClick.AddListener(
                    () =>
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    });

        }
        
    }
}
