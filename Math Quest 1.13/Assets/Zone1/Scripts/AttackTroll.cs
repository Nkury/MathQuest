using UnityEngine;
using System.Collections;

public class AttackTroll : MonoBehaviour
{
    public NPCWander wander;
    public static bool spawnTroll = true;
    public GameObject troll;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
        if (spawnTroll == true)
        {
            troll.SetActive(true);
            
                GameObject g = GameObject.FindGameObjectWithTag("troll");
                wander = g.GetComponent<NPCWander>();
                wander.attackTroll = true;
                spawnTroll = false;
            }
        }

    }
}