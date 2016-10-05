using UnityEngine;
using System.Collections;

public class townstart : MonoBehaviour {

	// Use this for initialization
	void Start () {

        if (cat.beaten)
        {
            QuestDelegate.primaryQuestUpdated();
            GameObject player = GameObject.Find("Cha_Knight");
            GameObject tutorial = GameObject.Find("MovementOverlay");
            tutorial.SetActive(false);
            player.transform.position = new Vector3(36.42f, 43.78f, 69.96f);
            player.transform.rotation = new Quaternion(-3.190491f, 120.2926f, 351.9924f, 1);
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
