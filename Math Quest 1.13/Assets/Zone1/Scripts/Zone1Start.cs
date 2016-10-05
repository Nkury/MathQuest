using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zone1Start : MonoBehaviour {

    // stores the names of the game objects that will be destroyed when the scene loads
    public List<string> destroyList = new List<string>();

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);

        if (MonsterCollider.enemyCollide)
        {
            float x_pos = PlayerPrefs.GetFloat("player_x");
            float y_pos = PlayerPrefs.GetFloat("player_y");
            float z_pos = PlayerPrefs.GetFloat("player_z");
            GameObject player = GameObject.Find("Main Character");
            player.transform.position = new Vector3(x_pos-5, y_pos+1.5f, z_pos-5);
            MonsterCollider.enemyCollide = false;
        }
	}

	// Update is called once per frame
	void Update () {
	}

    public void addToList(string name)
    {
        destroyList.Add(name);
    }
}
