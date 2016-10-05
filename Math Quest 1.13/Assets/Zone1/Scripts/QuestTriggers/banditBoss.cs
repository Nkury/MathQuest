//**Note: 11/12/15
//  Attach this script to banditBoss Trigger.
//  Attach banditBossText Object to 'G Bandit Boss Text' in the inspector. 
//  Nothing needs to be attached to the Text variable in the inspector. 

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class banditBoss : MonoBehaviour
{
    public static bool enableDynamite = false;
    public static bool banditBossScene = false;
    public GameObject gBanditBossText;
    public Text text;

    //************************************************************************************************
    void Start()
    {
        text = gBanditBossText.GetComponent<Text>();
    }

    //************************************************************************************************
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (banditBossScene == false)
            {
                //Text commented out, can re-add if desired. 
                //text.text = "The bandits blocked the path with a rock!\n" +
                //    "Go find some dynamite to blow it up!";
                banditBossScene = true;
                enableDynamite = true;
            }
            else if (dynamite.hasDynamite == false)
            {
                //text.text = "Stop stalling! Go find the dynamite!";
            }
            else
            {
                //text.text = "Now blow up the rock!";
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
	void Update () 
    {
	
	}
}
	

