//**Note: 11/12/15
//  Attach this script to any Invisible Walls. 
//  In the inspector you can enter a string that will set the text (if any). 

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class invisibleWalls : MonoBehaviour {

    public GameObject invisibleWallText;
    public Text text;
    public string wallType;

    //************************************************************************************************
    void OnTriggerEnter(Collider other)
    {
        text = invisibleWallText.GetComponent<Text>();

        if (other.gameObject.tag == "Player")
        {

        }
    }

    //************************************************************************************************
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            text.text = "";
        }
    }

    //************************************************************************************************
	void Start () 
    {
        
	}

    //************************************************************************************************
	// Update is called once per frame
	void Update () 
    {
        if (this.tag == "exittutorial" && cat.beaten)
        {
            this.GetComponent<Collider>().enabled = false;
        }
	}
}
