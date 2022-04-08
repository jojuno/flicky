using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    //color code
    private int player1Score = 5;
    //private int player2Score = 0;
    public Text player1ScoreText;
    //public Text player2ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player1ScoreText.text = "Player 1 Score: " + player1Score;
        //player2ScoreText.text = "Player 2 Score: " + player2Score;

        if (Input.GetKeyDown(KeyCode.Space)) {
            player1Score--;
        }

    }
}
