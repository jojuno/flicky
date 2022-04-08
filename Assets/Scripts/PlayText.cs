using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayText : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(PlayOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayOnClick()
    {
        GameMaster2 gm = GameObject.Find("board").GetComponent<GameMaster2>();
        gm.PlayClicked = true;
    }
}