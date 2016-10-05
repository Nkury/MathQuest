using UnityEngine;
using System.Collections;

public class SampleAgentScript : MonoBehaviour
{
    public Transform target;
    public Transform target2;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(target.position);
        agent.SetDestination(target2.position);
    }


}
