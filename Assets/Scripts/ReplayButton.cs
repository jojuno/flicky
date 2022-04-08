using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayButton : MonoBehaviour
{
    private bool clicked;
    public bool Clicked
    {
        get { return clicked; }
        set { clicked = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        clicked = false;
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnButtonPress);
    }

    public void OnButtonPress()
    {
        //reset everything
        clicked = true;
        this.GetComponent<Text>().enabled = false;
        this.GetComponent<Button>().enabled = false;
        GameObject.Find("board").GetComponent<GameMaster2>().Reset();
    }
}