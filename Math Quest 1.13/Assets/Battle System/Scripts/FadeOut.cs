using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour {

    bool fadeOut = false;
	// Use this for initialization
	void Start () {
     	}
	
	// Update is called once per frame
	void Update () {
        if(fadeOut)
            this.transform.localScale = new Vector3(this.transform.localScale.x + .15f, this.transform.localScale.y + .15f,
                                          this.transform.localScale.z + .15f);
	}

    public void startFadeOut()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
        fadeOut = true;
    }
}
