using UnityEngine;
using System.Collections;

public class QuestDelegate : MonoBehaviour {

	// Delegates
	public delegate void PrimaryQuestProgressHandler();
	public delegate void MasteryQuestProgressHandler(int medals);
	public delegate void StartQuestForZoneHandler(int zoneNum);

	// Events
	public static event PrimaryQuestProgressHandler onPrimaryQuestProgressEvent;
	public static event MasteryQuestProgressHandler onMasteryQuestProgressEvent;
	public static event StartQuestForZoneHandler onStartQuestForZoneEvent;


	public static void primaryQuestUpdated() {
		if (onPrimaryQuestProgressEvent != null) {
			onPrimaryQuestProgressEvent();
		}
	}

	public static void masteryQuestUpdated(int medals) {
		if (onMasteryQuestProgressEvent != null) {
			onMasteryQuestProgressEvent(medals);
		}
	}

	public static void startQuestForZone(int zoneNum) {
		if (onStartQuestForZoneEvent != null) {
			onStartQuestForZoneEvent(zoneNum);
		}
	}
}
