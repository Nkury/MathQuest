using UnityEngine;
using System.Collections;

public class MinimapScript : MonoBehaviour {

	public GameObject player;
	public Vector3 followVector;
	public int offset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		followVector = new Vector3 (player.transform.position.x, player.transform.position.y + offset, player.transform.position.z);
		transform.position = followVector;
	}
}
