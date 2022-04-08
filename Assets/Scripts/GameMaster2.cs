using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameMaster2 : MonoBehaviour
{
    private bool resetPiece = false;

    Plane m_Plane;
    public int turnNum;
    //must set default value in "Start()"
    public int turnsToBattle;
    public int currentPlayerId;
    public string mode;
    public string status;
    public bool stoneCreated;
    public bool stonePlaced;
    public int winner;
    public List<GameObject> pieces;
    protected Transform camTransform;
    protected Transform camPivotTransform;

    public Quaternion p1CamRotation;
    public Quaternion p1CamPivotRotation;
    public Vector3 p1CamPivotPosition;
    public Vector3 p1CamPosition;

    public Quaternion p2CamRotation;
    public Quaternion p2CamPivotRotation;
    public Vector3 p2CamPivotPosition;
    public Vector3 p2CamPosition;

    public GameObject cam;
    public GameObject camPivot;

    public GameObject pieceInHand;
    private bool pieceMoving;
    public bool PieceMoving
    {
        get { return pieceMoving; }
        set { pieceMoving = value; }
    }
    private bool flicked;
    public bool Flicked
    {
        get { return flicked; }
        set { flicked = value; }
    }

    //must use a private variable to interact with another script
    private bool playClicked;
    public bool PlayClicked
    {
        get { return playClicked; }
        set { playClicked = value; }
    }

    //pieces
    public GameObject bitcoin;
    public GameObject go;
    public GameObject dice;
    public GameObject pyramid;

    //arrow, because drag and shoot is assigned at runtime
    public GameObject dragIndicatorArrow;

    // Start is called before the first frame update
    void Start()
    {
        mode = "setup";
        currentPlayerId = 0;
        //for raycasting
        m_Plane = new Plane(Vector3.up, new Vector3(0, 3, 0));
        turnsToBattle = 10;
        turnNum = 1;
        playClicked = false;
        stoneCreated = false;
        stonePlaced = false;
        flicked = false;
        
        camTransform = cam.transform;
        camPivotTransform = camPivot.transform;

        Vector3 _LocalRotation = new Vector3(camPivotTransform.rotation.eulerAngles.y, camPivotTransform.rotation.eulerAngles.x, 0);
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        p1CamPivotRotation = QT;
        p1CamPivotPosition = camPivotTransform.position;
        p1CamRotation = camPivotTransform.rotation;
        p1CamPosition = camPivotTransform.position - camPivotTransform.forward * cam.GetComponent<CameraOrbit>()._CameraDistance;
        //place p2 camera opposite of p1
        Vector3 _LocalRotationOpposite = new Vector3(camPivotTransform.rotation.eulerAngles.y, camPivotTransform.rotation.eulerAngles.x, 0);
        Quaternion QTOpposite = Quaternion.Euler(_LocalRotationOpposite.y, _LocalRotationOpposite.x - 180, 0);
        p2CamPivotRotation = QTOpposite;
        p2CamRotation = p2CamPivotRotation;
        p2CamPivotPosition = camPivotTransform.position;
        p2CamPivotPosition = new Vector3(-p2CamPivotPosition.x, p2CamPivotPosition.y, -p2CamPivotPosition.z);
        p2CamPosition = camPivotTransform.position - camPivotTransform.forward * cam.GetComponent<CameraOrbit>()._CameraDistance;
        p2CamPosition = new Vector3(-p2CamPosition.x, p2CamPosition.y, -p2CamPosition.z);
    }

    public void Reset()
    {
        while (pieces.Count != 0)
        {
            Destroy(pieces[0]);
            pieces.RemoveAt(0);
        }
        turnNum = 1;
        mode = "setup";
        currentPlayerId = 0;
        flicked = false;
        //when set up is finished, the player 2 is set to flick first
        GameObject.Find("ReplayButton").GetComponent<ReplayButton>().Clicked = false;

    }

    //this runs in parallel with "Update"
    IEnumerator OnPieceDrop()
    {
        stonePlaced = true;

        //don't execute if piece touched another piece
        yield return new WaitForSeconds(0.5f);

        if (pieceInHand.transform.GetComponent<Piece>().Foul)
        {
            stonePlaced = false;
            resetPiece = true;
        }
        else
        {
            //replace with velocity
            Vector3 prevPos = pieceInHand.transform.position;
            Vector3 currPos = pieceInHand.transform.position;

            bool moving = true;
            while (moving)
            {
                prevPos = pieceInHand.transform.position;
                yield return new WaitForSeconds(0.1f);
                currPos = pieceInHand.transform.position;
                if (prevPos.x != currPos.x || prevPos.y != currPos.y || prevPos.z != currPos.z)
                    moving = true;
                else
                    moving = false;
            }
            //switch player turns
            SwitchCurrentPlayerId();
            turnNum++;
            if (turnNum == turnsToBattle) {
                mode = "battle";
            }
            stoneCreated = false;
        }
    }

    public void CreateStone(GameObject stone)
    {
        
        //depending on turnNum, create a coin, stone or a die
        pieceInHand = Instantiate(stone, new Vector3(0, 2, 0), Quaternion.identity);
        pieceInHand.AddComponent<Piece>();
        Renderer pieceRenderer = pieceInHand.GetComponent<Renderer>();
        if (stone.name != "pyramid") {
            if (currentPlayerId == 0)
            {
                pieceRenderer.materials[0].color = Color.red;
            }
            else
            {
                pieceRenderer.materials[0].color = Color.blue;
            }
            pieceInHand.GetComponent<Piece>().OwnerId = currentPlayerId;
        } else
        {
            pieceInHand.GetComponent<Piece>().OwnerId = -1;
        }
        pieces.Add(pieceInHand);
        stoneCreated = true;
        stonePlaced = false;
    }

    void DisplayStatus(string message)
    {
        StatusText statusText = GameObject.Find("Status").GetComponent<StatusText>();
        statusText.statusText.text = "";
        statusText.statusText.text = message;
    }

    void SwitchCurrentPlayerId()
    {
        if (currentPlayerId == 0)
        {
            //save previous player's camera
            Vector3 _LocalRotation = new Vector3(camPivotTransform.rotation.eulerAngles.y, camPivotTransform.rotation.eulerAngles.x, 0);
            Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
            p1CamPivotRotation = QT;
            p1CamPivotPosition = camPivotTransform.position;
            p1CamRotation = camPivotTransform.rotation;
            p1CamPosition = camPivotTransform.position - camPivotTransform.forward * cam.GetComponent<CameraOrbit>()._CameraDistance;
            
            currentPlayerId = 1;

            //place p2 camera opposite of p1
            // camTransform.rotation = p2CamRotation;
            camPivotTransform.rotation = p2CamPivotRotation;
            camPivotTransform.rotation = Quaternion.Lerp(camPivotTransform.rotation, p2CamPivotRotation, 1); // switches perspective immediately
            cam.GetComponent<CameraOrbit>()._LocalRotation = new Vector3(camPivotTransform.rotation.eulerAngles.y, camPivotTransform.rotation.eulerAngles.x, 0);
            camPivotTransform.position = p2CamPivotPosition;
            camTransform.position = p2CamPosition;
        }
        else
        {
            //save previous player's camera
            Vector3 _LocalRotation = new Vector3(camPivotTransform.rotation.eulerAngles.y, camPivotTransform.rotation.eulerAngles.x, 0);
            Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
            p2CamPivotRotation = QT;
            p2CamPivotPosition = camPivotTransform.position;
            p2CamRotation = camPivotTransform.rotation;
            p2CamPosition = camPivotTransform.position - camPivotTransform.forward * cam.GetComponent<CameraOrbit>()._CameraDistance;

            currentPlayerId = 0;

            //place p2 camera opposite of p1
            // camTransform.rotation = p2CamRotation;
            camPivotTransform.rotation = p1CamPivotRotation;
            camPivotTransform.rotation = Quaternion.Lerp(camPivotTransform.rotation, p1CamPivotRotation, 1); // switches perspective immediately
            cam.GetComponent<CameraOrbit>()._LocalRotation = new Vector3(camPivotTransform.rotation.eulerAngles.y, camPivotTransform.rotation.eulerAngles.x, 0);
            camPivotTransform.position = p1CamPivotPosition;
            camTransform.position = p1CamPosition;
        }
    }

    //method: count the number of pieces left on the board
    void CheckWinner()
    {
        
        int numPlayer1Pieces = 0;
        int numPlayer2Pieces = 0;
        foreach (GameObject piece in pieces)
        {
            if (piece.GetComponent<Piece>().OwnerId == 0)
            {
                numPlayer1Pieces++;
            }
            else if (piece.GetComponent<Piece>().OwnerId == 1)
            {
                numPlayer2Pieces++;
            }
        }
        if (numPlayer1Pieces > 0 && numPlayer2Pieces == 0)
        {
            winner = 0;
        }
        else if (numPlayer2Pieces > 0 && numPlayer1Pieces == 0)
        {
            winner = 1;
        }
        else if (numPlayer1Pieces == 0 && numPlayer2Pieces == 0)
        {
            winner = -2;
        } else
        {
            winner = -1;
        }
    }

    //need to set the state for every frame
    void Update()
    {
        if (playClicked)
        {
            if (mode == "setup")
            {

                if (stoneCreated == false)
                {
                    DisplayStatus(string.Format("Player {0}'s turn", currentPlayerId+1));
                    
                    if (turnNum == 1 || turnNum == 2) {
                        CreateStone(bitcoin);
                    }
                    else if (turnNum == 3 || turnNum == 4 || turnNum == 5 || turnNum == 6)
                    {
                        CreateStone(go);
                    }
                    else if (turnNum == 7 || turnNum == 8)
                    {
                        CreateStone(dice);
                    }
                    else if (turnNum == 9)
                    {
                        CreateStone(pyramid);
                    }

                }

                if (stoneCreated == true && stonePlaced == false)
                {
                    if (resetPiece)
                    {
                        pieceInHand.transform.rotation = Quaternion.identity;
                        pieceInHand.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        pieceInHand.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        resetPiece = false;
                    }


                    //if it's in the hand, it's not a foul
                    pieceInHand.transform.GetComponent<Piece>().Foul = false;

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    
                    //Initialise the enter variable
                    float enter = 1000.0f;

                    if (m_Plane.Raycast(ray, out enter))
                    {
                        Vector3 hitPoint = ray.GetPoint(enter);
                        pieceInHand.transform.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);

                        //flip it to make it flat with respect to the board if it's a go
                        if (pieceInHand.name == "goFlat6(Clone)" || pieceInHand.name == "pyramid(Clone)")
                        {
                            Vector3 newRotation = new Vector3(-90, 0, 0);
                            pieceInHand.transform.eulerAngles = newRotation;
                        }

                    }
                }

                if (Input.GetMouseButtonDown(0) && stonePlaced == false)
                {
                    if (pieceInHand.transform.position.x < -9 || pieceInHand.transform.position.x > 9 || pieceInHand.transform.position.z < -9 || pieceInHand.transform.position.z > 9)
                    {
                        DisplayStatus("Please place the piece on top of the board");
                    }
                    else
                    {
                        Quaternion QT = Quaternion.Euler(0, 0, 0);
                        pieceInHand.transform.rotation = Quaternion.Lerp(pieceInHand.transform.rotation, QT, 0f);
                        pieceInHand.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        StartCoroutine(OnPieceDrop());
                    }
                }
            }
            else if (mode == "battle") // battle
            {
                DisplayStatus(string.Format("Player {0}'s turn to flick", currentPlayerId + 1));
                bool pieceMoving = false;
                foreach (GameObject piece in pieces)
                {
                    if (piece.GetComponent<Rigidbody>().velocity != Vector3.zero)
                    {
                        pieceMoving = true;
                    }
                }
                //delayed setting "flicked" in shooting script
                //it takes a frame to check if the pieces are moving
                if (flicked && !pieceMoving)
                {
                    CheckWinner();
                    if (winner != -1)
                    {
                        mode = "finished";
                    }
                    else
                    {
                        SwitchCurrentPlayerId();
                        flicked = false;
                    }
                }
            }
            else if (mode == "finished")
            {
                
                if (winner == -2)
                {
                    DisplayStatus("Tie!");
                }
                else
                {
                    DisplayStatus(string.Format("Player {0} won!", winner + 1));
                }
                if (GameObject.Find("ReplayButton").GetComponent<ReplayButton>().Clicked == false)
                {
                    //if a game object is disabled, "Find" won't be able to find it
                    GameObject.Find("ReplayButton").GetComponent<Text>().enabled = true;
                    GameObject.Find("ReplayButton").GetComponent<Button>().enabled = true;
                }
            }
        }
    }
}
