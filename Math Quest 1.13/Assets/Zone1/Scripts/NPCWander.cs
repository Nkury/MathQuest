using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCWander : MonoBehaviour {

    public List<Transform> waypoints;
    public Transform target;
    public Transform target2;
    public Transform target3;

    public bool attackBandit = false;
    public bool attackTroll = false;

    NavMeshAgent agent;
    Transform player;

    // Use this for initialization
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        
        waypoints = new List<Transform>();
        waypoints.Add(target);
        waypoints.Add(target2);
        waypoints.Add(target3);

        StartCoroutine(MyCoroutine());
    }

    IEnumerator MyCoroutine()
    {


        if (gameObject.tag == "slime")
        {
            while (true)
            {
                agent.SetDestination(target.position);
                yield return new WaitForSeconds(8f);
                agent.SetDestination(target2.position);
                yield return new WaitForSeconds((Random.Range(5.0F, 10.0F)));
                agent.SetDestination(target3.position);
                yield return new WaitForSeconds(8f);
            }
        }

        if (gameObject.tag == "bandit")
        {
            while (true)
            {
                agent.SetDestination(target.position);
                yield return new WaitForSeconds(20f);
                agent.SetDestination(target2.position);
                yield return new WaitForSeconds((Random.Range(5.0F, 10.0F)));
                agent.SetDestination(target3.position);
                yield return new WaitForSeconds(15f);

            }
        }
    }

    void awake()
    {
        if (gameObject.tag == "attackBandit")
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            agent.SetDestination(player.position);
        }
        if (gameObject.tag == "troll")
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            agent.SetDestination(player.position);
        }

    }  

	 //Update is called once per frame
	    void Update () {
        if (attackBandit == true || attackTroll == true) {
         awake();
        }
	}
}
