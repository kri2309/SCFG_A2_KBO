using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class foodGenerator : MonoBehaviour
{
    positionRecord foodPosition;

    GameObject foodObject;

    List<positionRecord> allTheFood;

    GridGraph gg;

    snakeGenerator sn;

    public Vector3 enemyFoodPosition;
    bool EnemySpawned = false;
    private GameObject enemyFood;

    // Start is called before the first frame update
    void Start()
    {
        foodPosition = new positionRecord();

        allTheFood = new List<positionRecord>();

        foodObject = Resources.Load<GameObject>("Prefabs/Square");

        sn = Camera.main.GetComponent<snakeGenerator>();

        StartCoroutine(generateFood());
        gg = FindObjectOfType<AstarPath>().data.gridGraph;

        StartCoroutine(SpawnEnemy());
        GenerateEnemyFood();

    }

    int getVisibleFood()
    {
        int counter = 0;
        foreach (positionRecord f in allTheFood)
        {
            if (f.BreadcrumbBox != null)
            {
                counter++;
            }
        }

        return counter;
    }

    public void eatFood(Vector3 snakeHeadPosition, snakeGenerator sg)
    {
        positionRecord snakeHeadPos = new positionRecord();

        snakeHeadPos.Position = snakeHeadPosition;

        int foodIndex = allTheFood.IndexOf(snakeHeadPos);


        if (foodIndex != -1 && enemyFoodPosition != snakeHeadPos.Position)
        {
            // Debug.Log("Eating Food");

            Destroy(allTheFood[foodIndex].BreadcrumbBox);

            allTheFood.RemoveAt(foodIndex);

            sg.snakelength++;

        }
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(3);

        GameObject snake = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), enemyFoodPosition, Quaternion.identity);
        snake.name = "Enemy Robot";
        EnemySpawned = true;
        Destroy(enemyFood);
        yield return null;

    }

    IEnumerator generateFood()
    {
        while (true)
        {
            if (getVisibleFood() < 6)
            {
                yield return new WaitForSeconds(Random.Range(1f, 3f));

                foodPosition = new positionRecord();

                float randomX = Mathf.Floor(Random.Range(0f, 20f));

                float randomY = Mathf.Floor(Random.Range(0f, 20f));

                Vector3 randomLocation = new Vector3(randomX, randomY);

                //don't allow the food to be spawned on other food

                foodPosition.Position = randomLocation;

                if (!allTheFood.Contains(foodPosition) && !sn.hitTail(foodPosition.Position, sn.snakelength) && (gg.GetNode((int)randomX, (int)randomY).Walkable))

                {

                    foodPosition.BreadcrumbBox = Instantiate(foodObject, randomLocation, Quaternion.Euler(0f, 0f, 45f));


                    //make the food half the size
                    foodPosition.BreadcrumbBox.transform.localScale = new Vector3(0.5f, 0.5f);


                    foodPosition.BreadcrumbBox.GetComponent<SpriteRenderer>().color = Color.blue;


                    foodPosition.BreadcrumbBox.name = "Food Object";

                    allTheFood.Add(foodPosition);
                }

                yield return null;
            }


            yield return null;

        }
    }

    void GenerateEnemyFood()
    {
        foodPosition = new positionRecord();

        foodPosition.Position = enemyFoodPosition;

        foodPosition.BreadcrumbBox = Instantiate(foodObject, foodPosition.Position, Quaternion.Euler(0f, 0f, 45f));
        enemyFood = foodPosition.BreadcrumbBox;

        //make the food half the size
        foodPosition.BreadcrumbBox.transform.localScale = new Vector3(0.5f, 0.5f);


        foodPosition.BreadcrumbBox.GetComponent<SpriteRenderer>().color = Color.blue;


        foodPosition.BreadcrumbBox.name = "Enemy Food";
    }
}
