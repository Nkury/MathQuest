using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Destroyer : MonoBehaviour {

    public GameObject dynamiteObj;
    public GameObject rockObj;

	// Use this for initialization
	void Start () {
        GameObject zone1storage = GameObject.Find("PersistentZone1");
        List<string> dList = zone1storage.GetComponent<Zone1Start>().destroyList;

        // if the list of names goes over 3, then respawn those enemies that the player fought in the beginning
        if (dList.Count > 3)
            dList.RemoveAt(0);

        // destroys all the objects the player has defeated
        foreach (string name in dList)
            Destroy(GameObject.Find(name));

        if (dynamite.hasDynamite)
        {
            Destroy(dynamiteObj);
            QuestDelegate.primaryQuestUpdated();
        }
        if (rock.rockDestroyed)
        {
            QuestDelegate.startQuestForZone(3);
            Destroy(rockObj);
        }
	}

	// Update is called once per frame
	void Update () {
    if (dynamite.hasDynamite)
    {
        QuestDelegate.primaryQuestUpdated();
    }
    if (rock.rockDestroyed)
    {
        QuestDelegate.startQuestForZone(3);
    }
	}
}
