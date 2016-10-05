using UnityEngine;
using System.Collections;

public class MonsterCollider : MonoBehaviour {

    public static bool enemyCollide = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision obj)
    {

        enemyCollide = true;
        PlayerPrefs.SetFloat("player_x", obj.gameObject.transform.position.x);
        PlayerPrefs.SetFloat("player_y", obj.gameObject.transform.position.y);
        PlayerPrefs.SetFloat("player_z", obj.gameObject.transform.position.z);
       
        if (obj.gameObject.tag == "Player" && gameObject.tag == "slime")
        {
            int enemyHealth = Random.Range(5, 10);
            PlayerPrefs.SetInt("enemy_health", enemyHealth);
            PlayerPrefs.SetInt("monster_type", 1);
            PlayerPrefs.SetString("monster_name", gameObject.name);
        }

        if (obj.gameObject.tag == "Player" && (gameObject.tag == "bandit" || gameObject.tag == "attackBandit"))
        {
            int enemyHealth = Random.Range(7, 11);
            PlayerPrefs.SetInt("enemy_health", enemyHealth);
            PlayerPrefs.SetInt("monster_type", 2);
            PlayerPrefs.SetString("monster_name", gameObject.name);
        }

        if (obj.gameObject.tag == "Player" && gameObject.tag == "troll")
        {
            int enemyHealth = Random.Range(5, 7);
            PlayerPrefs.SetInt("enemy_health", enemyHealth);
            PlayerPrefs.SetInt("monster_type", 3);
            PlayerPrefs.SetString("monster_name", gameObject.name);
        }

        Application.LoadLevel("nizar battle system");
    }

}
