using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool moving;
    private int ownerId;
    public int OwnerId
    {
        get { return ownerId; }
        set { ownerId = value; }
    }
    private bool foul;
    public bool Foul
    {
        get { return foul; }
        set { foul = value; }
    }
    private bool isTouchingTheBoard;
    public bool IsTouchingTheBoard
    {
        get { return isTouchingTheBoard; }
        set { isTouchingTheBoard = value; }
    }
    private bool isTouchingAPiece;
    private GameMaster2 gm;

    // Start is called before the first frame update
    void Start()
    {
        foul = false;
        //flicked = false;
        moving = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        //get game mode
        if (gm.mode == "setup")
        {
            if (collision.transform.GetComponent<Piece>())
            {
                StatusText statusText = GameObject.Find("Status").GetComponent<StatusText>();
                statusText.statusText.text = "Don't place a piece on top of another one";
                this.Foul = true;
            }
            //board
            if (collision.transform.GetComponent<GameMaster2>())
            {
                isTouchingTheBoard = true;
            }
        }
        else if (gm.mode == "battle")
        {
            //board
            Debug.Log("collision entered");
            if (collision.transform.GetComponent<GameMaster2>())
            {
                Debug.Log("with board");
                if (collision.contacts[0].point.y < 0.5)
                {
                    //don't do anything
                }
                else
                {
                    isTouchingTheBoard = true;
                }
                
            }
            if (collision.transform.GetComponent<Piece>())
            {
                Debug.Log("with a piece");
                isTouchingAPiece = true;
            }
        }
    }

    IEnumerator OnPieceKnockedOut(GameObject piece)
    {
        yield return new WaitForSeconds(2);
        //check if it's touching the board again
        if (!piece.GetComponent<Piece>().isTouchingTheBoard && !piece.GetComponent<Piece>().isTouchingAPiece)
        {
            if (piece.transform.position.x < -10 || piece.transform.position.x > 10 || piece.transform.position.z < -10 || piece.transform.position.z > 10)
            {
                Debug.Log(piece.name + " was destroyed");
                gm.pieces.Remove(piece);
                Destroy(piece);
            } 
            else
            {
                Debug.Log("inbound");
            }

        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (gm.mode == "battle")
        {
            //board
            if (collision.transform.GetComponent<GameMaster2>())
            {
                Debug.Log("piece got knocked out: " + this.gameObject.name + "with owner id " + this.ownerId);
                Debug.Log("collision exit with: " + collision.transform.name);
                isTouchingTheBoard = false;
                
                StartCoroutine(OnPieceKnockedOut(this.gameObject));
            }
            if (collision.transform.GetComponent<Piece>())
            {
                isTouchingAPiece = false;
            }
        }
    }

    void Update()
    {
        gm = GameObject.Find("board").GetComponent<GameMaster2>();
        if (gm.Flicked)
        {
            if (this.transform.GetComponent<Rigidbody>().velocity != Vector3.zero)
            {
                moving = true;
            }
            else
            {
                moving = false;
            }
        }
    }
}

