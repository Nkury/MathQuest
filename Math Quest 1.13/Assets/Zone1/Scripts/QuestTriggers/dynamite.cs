//**Note: 11/12/15
//  Attach this script to findDynamite Trigger.
//  Attach dynamiteText Object to 'G Dynamite Text' in the inspector. 
//  Nothing needs to be attached to the Text variable in the inspector. 

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class dynamite : MonoBehaviour
{

    public static bool hasDynamite = false;
    public static bool troll = true;
    public GameObject gDynamiteText;
    public Text text;

    //************************************************************************************************
    void Start()
    {
        QuestDelegate.startQuestForZone(2);
        text = gDynamiteText.GetComponent<Text>();
    }

    //************************************************************************************************
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (banditBoss.enableDynamite == false)
            {

            }
            if (banditBoss.enableDynamite == true)
            {
                hasDynamite = true;
                if (troll == true)
                {
                    troll = false;
                }
            }
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
    // Update is called once per frame
    void Update()
    {

    }
}
