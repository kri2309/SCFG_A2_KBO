using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemySnakeScript : MonoBehaviour
{

   public snakeGenerator mysnakegenerator, sgPlayer;

    public float Shpeed = 1;

    //the object that we are using to generate the path
    Seeker seeker;

    //path to follow stores the path
    Path pathToFollow;

    //a reference from the UI to the green box
    public Transform target;

    //a reference to PointGraphObject
    GameObject graphParent;

    public List<Transform> obstacleNode;
    GameObject GameOver;
    foodGenerator fg;

    // public GameObject Obstacle; 

    // Start is called before the first frame update
    void Start()
    {
        fg = Camera.main.GetComponent<foodGenerator>();
        mysnakegenerator = GetComponent<snakeGenerator>();

        target = GameObject.Find("Black player box").transform;
        sgPlayer = Camera.main.GetComponent<snakeGenerator>();

        //the instance of the seeker attached to this game object
        seeker = GetComponent<Seeker>();

        //find the parent node of the point graph
        graphParent = GameObject.Find("Grid");

        //we scan the graph to generate it in memory
        graphParent.GetComponent<AstarPath>().Scan();

        //generate the initial path
        pathToFollow = seeker.StartPath(transform.position, target.position);

        //update the graph as soon as you can.  Runs indefinitely
        StartCoroutine(updateGraph());

        //move the red robot towards the green enemy
        StartCoroutine(moveTowardsEnemy(this.transform));
    }


    IEnumerator updateGraph()
    {
        while (true)
        {


            graphParent.GetComponent<AstarPath>().Scan();


            yield return null;

        }

    }


    IEnumerator moveTowardsEnemy(Transform t)
    {

        while (true)
        {

            List<Vector3> posns = pathToFollow.vectorPath;

            for (int counter = 0; counter < posns.Count; counter++)
            {
                while (Vector3.Distance(t.position, posns[counter]) >= 0.5f)
                {

                    mysnakegenerator.savePosition();
                    mysnakegenerator.drawTail(mysnakegenerator.snakelength);
                    t.position = Vector3.MoveTowards(t.position, posns[counter], 1f);
                    mysnakegenerator.ScanGrid();
                    fg.eatFood(this.transform.position, mysnakegenerator);
                    if (sgPlayer.hitTail(t.position, sgPlayer.snakelength))
                    {
                        GameLost();
                    }
                    //since the enemy is moving, I need to make sure that I am following him
                    pathToFollow = seeker.StartPath(t.position, target.position);
                    //wait until the path is generated
                    yield return seeker.IsDone();
                    //if the path is different, update the path that I need to follow
                    posns = pathToFollow.vectorPath;


                    yield return new WaitForSeconds(Shpeed);
                }
                //keep looking for a path because if we have arrived the enemy will anyway move away
                //This code allows us to keep chasing
                pathToFollow = seeker.StartPath(t.position, target.position);
                yield return seeker.IsDone();
                posns = pathToFollow.vectorPath;
                

            }
            yield return null;
        }
    }
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

}



