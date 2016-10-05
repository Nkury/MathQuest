using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

    public GameObject instructions;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayButtonClicked()
    {
        Application.LoadLevel("final Zone #1");
    }

    public void QuitButtonClicked()
    {
        Application.Quit();
    }

    public void showInstructions()
    {
        instructions.SetActive(true);
    }
}
