//**Note: 11/12/15
//  Attach this script to the rock Trigger.
//  Attach rockText Object to 'G Rock Text' in the inspector. 
//  Nothing needs to be attached to the Text variable in the inspector. 

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class rock : MonoBehaviour
{
    public static bool rockDestroyed = false;
    public GameObject gRockText;
    public GameObject explosion1;
    public GameObject explosion2;
    public GameObject explosion3;
    public GameObject camera;

    public Text text;


    //************************************************************************************************
    void Start()
    {
        text = gRockText.GetComponent<Text>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            if (dynamite.hasDynamite == true)
            {
                //At this point I wanted the quest icon to update to the spider
                //but the below code causes the rest of this if statement not to
                //executee for some reason.
                //QuestDelegate.startQuestForZone(2);
                rockDestroyed = true;
                StartCoroutine(DestroyRock());
       
            }
        }
    }

    IEnumerator DestroyRock()
    {
        AudioSource[] audios = camera.GetComponents<AudioSource>();
        audios[1].Play(); // play explosion sound effect
        explosion1.SetActive(true);
        yield return new WaitForSeconds(.5f);
        explosion2.SetActive(true);
        yield return new WaitForSeconds(.5f);
        explosion3.SetActive(true);
        Destroy(gameObject);
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