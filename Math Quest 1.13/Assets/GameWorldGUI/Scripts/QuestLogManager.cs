using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;

public class QuestLogManager : MonoBehaviour {

	// Quest Log Images
	public Image primaryQuestImage1; // Zone 1
	public Image primaryQuestImage2; // Zone 2
	public Image primaryQuestImage3; // Zone 3

	public Image primaryQuestBackgroundImage; // Quest Status
	public Text masteryQuestCount; // Number of Medals

	public float FadeDuration = 1f;

	private float lastColorChangeTime;
	private int currentPrimaryQuest = 0;
	private int finalPrimaryQuest = 3;

	public static int currentMasteryProgress = 0;
	private static int goalMasteryProgress = 0;

	private Color colorPrimaryQuestComplete = new Color(0, 1, 0, 1);
	private Color colorPrimaryQuestIncomplete = new Color(1, 0, 0, 1);

	private Color startColor = new Color(1, 0, 0, 1);
	private Color endColor = new Color(1, 1, 0, 1);

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);

		// Subscribe to events
		//QuestDelegate.onMasteryQuestProgressEvent += this.masteryQuestStatusUpdated;
		QuestDelegate.onPrimaryQuestProgressEvent += this.primaryQuestStatusUpdated;
		QuestDelegate.onStartQuestForZoneEvent += this.startQuestForZone;

	//	initMasteryProgress(0, 1000);
		startQuestForZone(1);
	}

	// Update is called once per frame
	void Update () {
        masteryQuestCount.text = Convert.ToString(currentMasteryProgress);
      

		if (primaryQuestBackgroundImage.color != colorPrimaryQuestComplete) {
			var ratio = (Time.time - lastColorChangeTime) / FadeDuration;
			ratio = Mathf.Clamp01(ratio);
			primaryQuestBackgroundImage.color = Color.Lerp(startColor, endColor, Mathf.Sqrt(ratio));
         
			if (ratio == 1f) {
				lastColorChangeTime = Time.time;

				// Switch colors
				var temp = startColor;
				startColor = endColor;
				endColor = temp;
			}
		}
	}

	void OnDisable () {
	//	QuestDelegate.onMasteryQuestProgressEvent -= this.masteryQuestStatusUpdated;
		QuestDelegate.onPrimaryQuestProgressEvent -= this.primaryQuestStatusUpdated;
		QuestDelegate.onStartQuestForZoneEvent -= this.startQuestForZone;
	}


	public void startQuestForZone(int zoneNum) {
		switch(zoneNum) {
		case 1:
			primaryQuestImage1.enabled = true;
			primaryQuestImage2.enabled = false;
			primaryQuestImage3.enabled = false;
			break;
		case 2:
			primaryQuestImage1.enabled = false;
			primaryQuestImage2.enabled = true;
			primaryQuestImage3.enabled = false;
			break;
		case 3:
			primaryQuestImage1.enabled = false;
			primaryQuestImage2.enabled = false;
			primaryQuestImage3.enabled = true;
			break;
		default:
			primaryQuestImage1.enabled = true;
			primaryQuestImage2.enabled = false;
			primaryQuestImage3.enabled = false;
			break;
		}
		primaryQuestBackgroundImage.color = colorPrimaryQuestIncomplete;
	}

	public void initMasteryProgress(int initial, int goal) {
		currentMasteryProgress = initial;
		goalMasteryProgress = goal;
	//	masteryQuestCount.text = Convert.ToString(initial);
	}

	public void primaryQuestStatusUpdated() {
		primaryQuestBackgroundImage.color = colorPrimaryQuestComplete;
	}


	public static void masteryQuestStatusUpdated(int medals) {
	//	if (currentMasteryProgress < goalMasteryProgress) {
      
			currentMasteryProgress+=medals;
	//	}
	}
}
