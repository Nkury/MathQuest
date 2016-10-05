//**Note: 11/12/15
//  Attach this script to nextLevel trigger object. 
//  Specify the next level to load in the inspector.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class nextLevel : MonoBehaviour
{

    public string levelToLoad;
    public GameObject gLevelText;
    public Text text;
    public bool spiderFight;
    //************************************************************************************************
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Use this code if Zone1 should go back to Tutorial Zone
            /*
            if (dynamite.hasDynamite == true)
            {
                text.text = "Spiders rush out of the cave and head to your " +
                "village\n! You must go stop them first!";
                StartCoroutine(MyCoroutine());
            }
            */

            //This code assumes from Zone1 the player is just going to battle
            //the final bass instead of doing the spider fighting in Tutorial Zone. 
            if (dynamite.hasDynamite == true && spiderFight)
            {
                StartCoroutine(MyCoroutine());
                QuestDelegate.primaryQuestUpdated();
                
            }
            //Code to load Zone1 from Tutorial Zone
            else
            {
                Application.LoadLevel(levelToLoad);

            }
        }
    }

    IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(5f);
        Application.LoadLevel(levelToLoad);
    }



    //************************************************************************************************
    // Use this for initialization
    void Start()
    {
        text = gLevelText.GetComponent<Text>();
    }

    //************************************************************************************************
    // Update is called once per frame
    void Update()
    {

    }
}
