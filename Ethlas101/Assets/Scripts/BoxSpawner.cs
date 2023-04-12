using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class BoxSpawner : MonoBehaviour
{
    //GameObjects
    public GameObject CameraGO;
    public GameObject GameOverScreenGO;
    public GameObject MainMenuScreenGO;

    public GameObject BoxPrefab;
    public GameObject BaseBoxGO;
    public GameObject SpawnedBoxGO;
    public GameObject PreviousBoxGO;

    public BoxController BoxScript;
    public Rigidbody BoxRigidBody;

    public Text CurrentScore;
    public Text FinalScore;
    int ScoreCounter;

    //Gamestate controller
    public int GameState = 0;

    //Alternates between X and Z spawns
    int SpawnLocation = 0;

    //Determines how high to spawn the next box
    float SpawnHeight = 0;

    //Container to hold integer to cycle through list of materials 
    int BoxColorIterator = 1;

    //Box travelling speed
    float Speed = -10.0f;

    //Initial scale and position values
    float NewScaleX = 12.0f;
    float NewScaleZ = 12.0f;
    float NewPositionX = 6.0f;
    float NewPositionZ = 6.0f;


    // Start is called before the first frame update
    void Start()
    {
        //Initial GameState
        GameState = 1;

        //Initializes PreviousBoxGO as BaseBoxGO
        PreviousBoxGO = BaseBoxGO;

        //Get SpawnHeight based off this current gameobject's height
        SpawnHeight = this.gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Runs the logic to spawn new boxes after primary mouse click
        if (Input.GetMouseButtonDown(0))
        {
            if (MainMenuScreenGO.activeSelf == false)
            {
                SpawnBoxes();
            }
        }
    }

    void SpawnBoxes()
    {
        //GameState at 1
        if (GameState == 1)
        {
            //Increases Camera height based off each box height
            CameraGO.GetComponent<CameraControls>().IncreaseCameraHeight();
            SpawnHeight++;
            //Instantiates a new box with along the current X and Z axis of the previous box
            SpawnedBoxGO = Instantiate(BoxPrefab, new Vector3(NewPositionX, SpawnHeight, NewPositionZ), Quaternion.identity);
            //Sets X and Z scale of new box to previous box
            SpawnedBoxGO.transform.localScale = new Vector3(NewScaleX, 1, NewScaleZ);
            //Get rigidbody and BoxController script from new box
            BoxScript = SpawnedBoxGO.GetComponent<BoxController>();
            BoxRigidBody = SpawnedBoxGO.GetComponent<Rigidbody>();
            //Switches between X and Z axis spawns
            switch (SpawnLocation)
            {
                //Spawns a box on X axis
                case 0:
                    {
                        //Sets spawn position of new box
                        SpawnedBoxGO.gameObject.transform.position = new Vector3(30, SpawnHeight, NewPositionZ);
                        //Sets new material of new box
                        SpawnedBoxGO.GetComponent<MeshRenderer>().material = SpawnedBoxGO.GetComponent<BoxController>().GetColor(BoxColorIterator);
                        //Adds a force in X axis
                        BoxScript.AddForceX(BoxRigidBody, Speed);
                        //Goes to Z spawn next
                        SpawnLocation++;
                        break;
                    }
                //Spawns a box on Z axis
                case 1:
                    {
                        //Sets spawn position of new box
                        SpawnedBoxGO.gameObject.transform.position = new Vector3(NewPositionX, SpawnHeight, 30);
                        //Sets new material of new box
                        SpawnedBoxGO.GetComponent<MeshRenderer>().material = SpawnedBoxGO.GetComponent<BoxController>().GetColor(BoxColorIterator);
                        //Adds a force in Z axis
                        BoxScript.AddForceZ(BoxRigidBody, Speed);
                        //Goes to X spawn next
                        SpawnLocation = 0;
                        break;
                    }
                default:
                    break;
            }
            //Goes to next material to be used
            if (BoxColorIterator < 6)
            {
                BoxColorIterator++;
            }
            //Resets int to prevent going out of List range
            else
            {
                BoxColorIterator = 0;
            }
        }

        switch (GameState)
        {
            //GameState when player clicks to stop a box
            case 0:
                {
                    if (SpawnedBoxGO != null)
                    {
                        //Stops the box from moving after player clicks
                        SpawnedBoxGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
                        SpawnedBoxGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;

                        switch (SpawnLocation)
                        {
                            //Calculates which axis to scale the box by depending on its movement
                            case 0:
                                {
                                    CalculateCollisionZ(PreviousBoxGO, SpawnedBoxGO);
                                    SpawnedBoxGO.transform.position = new Vector3(NewPositionX, PreviousBoxGO.transform.position.y + 1.0f, NewPositionZ);
                                    break;
                                }
                            case 1:
                                {
                                    CalculateCollisionX(PreviousBoxGO, SpawnedBoxGO);
                                    SpawnedBoxGO.transform.position = new Vector3(NewPositionX, PreviousBoxGO.transform.position.y + 1.0f, NewPositionZ);
                                    break;
                                }
                            default:
                                break;
                        }
                        //Shows game over screen if box completely misses the previous box
                        if (SpawnedBoxGO.GetComponent<BoxController>().IsColliding == false)
                        {
                            GameOverScreenGO.SetActive(true);
                            CurrentScore.gameObject.SetActive(false);
                            FinalScore.text = "Congratulations!\nYour final score is:\n" + ScoreCounter;
                            GameState = 2;
                        }
                        //Scale and position the current box according to previous box position
                        else
                        {
                            PreviousBoxGO = SpawnedBoxGO;
                            SpawnedBoxGO.transform.localScale = new Vector3(NewScaleX, 1, NewScaleZ);
                            ScoreCounter++;
                            CurrentScore.text = "Score:\n" + ScoreCounter;
                            GameState++;
                        }
                    }
                    else GameState++;
                    break;
                }
            case 1:
                {
                    //Instantiates a new box
                    GameState = 0;
                    break;
                }
            case 2:
                {
                    break;
                }
        }
    }

    void CalculateCollisionX(GameObject oldBox, GameObject newBox)
    {
        // takes current platform's center + half the scale to get upper X value
        float oldUpperLimitX = oldBox.transform.position.x + oldBox.transform.localScale.x / 2;
        float newUpperLimitX = newBox.transform.position.x + newBox.transform.localScale.x / 2;

        // takes new platform's center - half the scale to get new lower X value
        float oldLowerLimitX = oldBox.transform.position.x - oldBox.transform.localScale.x / 2;
        float newLowerLimitX = newBox.transform.position.x - newBox.transform.localScale.x / 2;

        float upperLimitX = oldUpperLimitX < newUpperLimitX ? oldUpperLimitX : newUpperLimitX;
        float lowerLimitX = oldLowerLimitX > newLowerLimitX ? oldLowerLimitX : newLowerLimitX;

        // new lower limit + half the new scale for new center
        NewPositionX = lowerLimitX + (upperLimitX - lowerLimitX) / 2;
        NewScaleX = (upperLimitX - lowerLimitX);
    }

    void CalculateCollisionZ(GameObject oldBox, GameObject newBox)
    {
        // takes current platform's center + half the scale to get upper Z value
        float oldUpperLimitZ = oldBox.transform.position.z + oldBox.transform.localScale.z / 2;
        float newUpperLimitZ = newBox.transform.position.z + newBox.transform.localScale.z / 2;

        // takes new platform's center - half the scale to get new lower Z value
        float oldLowerLimitZ = oldBox.transform.position.z - oldBox.transform.localScale.z / 2;
        float newLowerLimitZ = newBox.transform.position.z - newBox.transform.localScale.z / 2;

        float upperLimitZ = oldUpperLimitZ < newUpperLimitZ ? oldUpperLimitZ : newUpperLimitZ;
        float lowerLimitZ = oldLowerLimitZ > newLowerLimitZ ? oldLowerLimitZ : newLowerLimitZ;

        // new lower limit + half the new scale for new center
        NewPositionZ = lowerLimitZ + (upperLimitZ - lowerLimitZ) / 2;
        NewScaleZ = (upperLimitZ - lowerLimitZ);
    }
}