using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreText : MonoBehaviour
{
    public Text scoreText;
    public int playerID;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        //textColor = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Text>().color = textColor;
        GameObject gm = GameObject.Find("GameMaster");
        //this game master is not the same one as the one defined in the script
        //how to access the score from GameMaster
        //gm.players[0].NumCoinsOnBoard = 10;
        //int score = gm.players[0].NumCoinsOnBoard;
        //int score = board.GetComponent<GameMaster>().players[0].NumCoinsOnBoard;
        //scoreText.text = "Player 1 coins: " + score;
    }
}