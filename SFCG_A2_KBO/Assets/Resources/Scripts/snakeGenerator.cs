using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pathfinding;


public class positionRecord
{
    //the place where I've been
    Vector3 position;
    //at which point was I there?
    int positionOrder;


    GameObject breadcrumbBox;
    public override bool Equals(System.Object obj)
    {
        if (obj == null)
            return false;
        positionRecord p = obj as positionRecord;
        if ((System.Object)p == null)
            return false;
        return position == p.position;
    }


    public bool Equals(positionRecord o)
    {
        if (o == null)
            return false;


        //the distance between any food spawned
        return Vector3.Distance(this.position, o.position) < 2f;


    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }




    public Vector3 Position { get => position; set => position = value; }
    public int PositionOrder { get => positionOrder; set => positionOrder = value; }
    public GameObject BreadcrumbBox { get => breadcrumbBox; set => breadcrumbBox = value; }
}

public class snakeGenerator : MonoBehaviour
{

    public int snakelength;

    foodGenerator fgen;
    snakeheadController snakeController;

    GridGraph gg;

    int pastpositionslimit = 100;

    public GameObject playerBox, breadcrumbBox, pathParent, timerUI;

    List<positionRecord> pastPositions;

    int positionorder = 0;

    bool firstrun = true;

    GameObject GameOver;
  

    public  Color snakeColor;

    public string NextLevel;
    
    public bool Enemy;


    // Start is called before the first frame update
    void Start()
    {

        snakeheadController snakehead = null;
        gg = FindObjectOfType<AstarPath>().data.gridGraph;

        if (!Enemy)
        {
            
            snakeColor = Color.white;

            playerBox = Instantiate(Resources.Load<GameObject>("Prefabs/Square"), new Vector3(1, 15), Quaternion.identity);

            // timerUI = Instantiate(Resources.Load<GameObject>("Prefabs/Timer"), new Vector3(0f, 0f), Quaternion.identity);

            //the default value for the timer is started
            if (FindObjectOfType<timerManager>()) { FindObjectOfType<timerManager>().timerStarted = true; }
           
           // timerUI.GetComponentInChildren<timerManager>().timerStarted = true;
           
           

            playerBox.GetComponent<SpriteRenderer>().color = Color.black;


            //move the box with the arrow keys
            snakehead = playerBox.AddComponent<snakeheadController>();
            snakehead.NextLevel = NextLevel;

            playerBox.name = "Black player box";

            

            fgen = Camera.main.GetComponent<foodGenerator>();

            // StartCoroutine(waitToGenerateFood());


        }
        else
        {
            
            playerBox = this.gameObject;
            snakeColor = Color.red;
            
        }

        pathParent = new GameObject();

        pathParent.transform.position = new Vector3(0f, 0f);

        pathParent.name = "Path Parent";


        breadcrumbBox = Resources.Load<GameObject>("Prefabs/Square");
        pastPositions = new List<positionRecord>();
        if (!Enemy ) {
            if (snakehead.NextLevel == "Ending")
            {
                snakelength = PlayerPrefs.GetInt("snakelength");
            }
        
        }
        drawTail(snakelength);

    }

    public void ScanGrid()
    {

        gg.Scan();
    }

    public void savePosition()
    {
       
        if (playerBox)
        {
            positionRecord currentBoxPos = new positionRecord();
            currentBoxPos.Position = playerBox.transform.position;
            positionorder++;
            currentBoxPos.PositionOrder = positionorder;

            pastPositions.Add(currentBoxPos);
        }

    


    }


    void cleanList()
    {
        for (int counter = pastPositions.Count - 1; counter > pastPositions.Count; counter--)
        {
            pastPositions[counter] = null;
        }
    }



    public void drawTail(int length)
    {
        clearTail();

        if (pastPositions.Count > length)
        {
            //nope
            //I do have enough positions in the past positions list
            //the first block behind the player
            int tailStartIndex = pastPositions.Count - 1;
            int tailEndIndex = tailStartIndex - length;


            //if length = 4, this should give me the last 4 blocks
            for (int snakeblocks = tailStartIndex; snakeblocks > tailEndIndex; snakeblocks--)
            {

                pastPositions[snakeblocks].BreadcrumbBox = Instantiate(breadcrumbBox, pastPositions[snakeblocks].Position, Quaternion.identity);
                pastPositions[snakeblocks].BreadcrumbBox.GetComponent<SpriteRenderer>().color = snakeColor;
                pastPositions[snakeblocks].BreadcrumbBox.AddComponent<BoxCollider2D>();
                pastPositions[snakeblocks].BreadcrumbBox.layer = LayerMask.NameToLayer("Obstacle");

            }

        }

        if (firstrun)
        {

            //I don't have enough positions in the past positions list
            for (int count = length; count > 0; count--)
            {
                positionRecord fakeBoxPos = new positionRecord();
                float ycoord = count * -1;
                fakeBoxPos.Position = new Vector3(0f, ycoord);
                // Debug.Log(new Vector3(0f, ycoord));
                //fakeBoxPos.BreadcrumbBox = Instantiate(breadcrumbBox, fakeBoxPos.Position, Quaternion.identity);
                //fakeBoxPos.BreadcrumbBox.GetComponent<SpriteRenderer>().color = Color.yellow;
                pastPositions.Add(fakeBoxPos);




            }
            firstrun = false;
            drawTail(length);
            //Debug.Log("Not long enough yet");
        }

    }


    //if hit tail returns true, the snake has hit its tail
    public bool hitTail(Vector3 headPosition, int length)
    { 
        int tailStartIndex = pastPositions.Count - 1;
        int tailEndIndex = tailStartIndex - length;
        

        //I am checking all the positions in the tail of the snake
        for (int snakeblocks = tailStartIndex; snakeblocks > tailEndIndex; snakeblocks--)
        {
            if ((headPosition == pastPositions[snakeblocks].Position) && (pastPositions[snakeblocks].BreadcrumbBox != null))
            {
                Debug.Log("Hit Tail " + this.name);

                GameOver = Instantiate(Resources.Load<GameObject>("Prefabs/ButtonPrefab"), new Vector3(0f, 0f), Quaternion.identity);

                GameOver.GetComponentInChildren<Text>().text = "You Lost!";

                GameOver.GetComponentInChildren<Button>().onClick.AddListener(
                        () =>
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        });

                return true;
            }
        }

            return false;

    }



    void clearTail()
    {
        cleanList();
        foreach (positionRecord p in pastPositions)
        {
            // Debug.Log("Destroy tail" + pastPositions.Count);
            Destroy(p.BreadcrumbBox);
        }
    }





    void Update()
    {
        if (Input.anyKeyDown && !((Input.GetMouseButtonDown(0)
            || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))) && !Input.GetKeyDown(KeyCode.X) && !Input.GetKeyDown(KeyCode.Z) && !Input.GetKeyDown(KeyCode.Space) && !Enemy)
        {

            savePosition();

            //draw a tail of length 4
            drawTail(snakelength);

        }

    }
}
