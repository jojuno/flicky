using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusText : MonoBehaviour
{
    //color code
    private int currentPlayerNumber = 1;
    private string phase = "place";
    private int stonesPlaced = 0;
    public Text statusText;
    private Color textColor;

    // Start is called before the first frame update
    void Start()
    {
        //statusText.text = "";
        //textColor = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        GetComponent<Text>().color = textColor;

        if (phase.Equals("place"))
        {
            if (stonesPlaced == 1)
            {
                statusText.text = "red doge place your doge";
                textColor = Color.red;
                currentPlayerNumber = 2;
                stonesPlaced++;
            } else if (stonesPlaced == 2)
            {
                statusText.text = "blue doge place your ethi";
                textColor = Color.blue;
                currentPlayerNumber = 1;
                stonesPlaced++;
            } else if (stonesPlaced == 3)
            {
                statusText.text = "red doge place your ethi";
                textColor = Color.red;
                currentPlayerNumber = 2;
                stonesPlaced++;
            } else if (stonesPlaced == 4)
            {
                statusText.text = "blue doge place your bitco";
                textColor = Color.blue;
                currentPlayerNumber = 1;
                stonesPlaced++;
            } else if (stonesPlaced == 5)
            {
                statusText.text = "red doge place your bitco";
                textColor = Color.red;
                currentPlayerNumber = 2;
                stonesPlaced++;
            } else if (stonesPlaced == 6)
            {
                phase = "flick";
                textColor = Color.blue;
                currentPlayerNumber = 1;
                stonesPlaced++;
            }
            
            
        //display who flicks a coin
        } else if (phase.Equals("flick"))
        {
            if (currentPlayerNumber == 1)
            {
                statusText.text = "red doge flick your coin";
            }
            else if (currentPlayerNumber == 2)
            {
                statusText.text = "blue doge flick your coin";
            }
        }
        */
    }
}
