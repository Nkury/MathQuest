using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class medalCount : MonoBehaviour {

    public Text medalcount;
    public Text title;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
      medalcount.text = QuestLogManager.currentMasteryProgress.ToString();
      if (QuestLogManager.currentMasteryProgress <= 5)
      {
          title.text = "Rookie";
      } else if(QuestLogManager.currentMasteryProgress <= 15)
      {
          title.text = "Matheteur";
      }
      else if (QuestLogManager.currentMasteryProgress <= 25)
      {
          title.text = "Wizard";
      }
      else
      {
          title.text = "Math Genius";
      }
	}
}
