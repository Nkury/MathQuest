using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cat : MonoBehaviour
{


    public GameObject catText;
    public Text text;
    public GameObject obj; // player
    public static bool beaten = false;

    void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "Player" && !beaten)
        {
            QuestDelegate.primaryQuestUpdated();
            PlayerPrefs.SetInt("enemy_health", 2);
            PlayerPrefs.SetInt("monster_type", 4);
            Application.LoadLevel("nizar battle system");

        }
    }

    //************************************************************************************************
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            text.text = "";
            QuestDelegate.primaryQuestUpdated();
        }
    }

    // Use this for initialization
    void Start()
    {
        //QuestDelegate.startQuestForZone(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

