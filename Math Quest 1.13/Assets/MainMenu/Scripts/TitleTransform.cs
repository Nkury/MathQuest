using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TitleTransform : MonoBehaviour {

    Text TitleText;

	// Use this for initialization
	void Start ()
    {
        RectTransform canvas = gameObject.transform.root.transform as RectTransform;
        TitleText = gameObject.GetComponent<Text>();

        float heightMargin = canvas.sizeDelta.y * .30f;
        float widthMargin = canvas.sizeDelta.x * .05f;

        TitleText.rectTransform.position = new Vector2(widthMargin, TitleText.rectTransform.position.y + heightMargin);
        TitleText.rectTransform.sizeDelta = new Vector2(canvas.sizeDelta.x * .5f, canvas.sizeDelta.y);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
