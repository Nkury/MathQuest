using UnityEngine;
using System.Collections;

public class AttackBandit : MonoBehaviour {
    public NPCWander wander;
    public static bool isAlive = true; 
	

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isAlive == true)
        {
            GameObject g = GameObject.FindGameObjectWithTag("attackBandit");
            wander = g.GetComponent<NPCWander>();
            wander.attackBandit = true;
            isAlive = false;
        }

    }
}
