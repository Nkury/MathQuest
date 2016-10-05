using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class dynamiteObject : MonoBehaviour
{

    public float rotationSpeed;

    // Use this for initialization
    void Start()
    {
        rotationSpeed = 100;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            dynamite.hasDynamite = true;
            Destroy(gameObject);
            QuestDelegate.startQuestForZone(3);
        }
    }
}
