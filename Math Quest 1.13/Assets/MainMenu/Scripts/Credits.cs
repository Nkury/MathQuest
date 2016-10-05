using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(UnityEngine.UI.Text))]
public class Credits : MonoBehaviour {

    public readonly float Y_SCROLL = .95f;

    Vector2 initialPosition;
    Text creditsText;

    bool ShowingCredits;
	// Use this for initialization
	void Start ()
    {
        creditsText = gameObject.GetComponent<Text>();
        initialPosition = creditsText.rectTransform.position;
        Init();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!IsFinished && ShowingCredits)
        {
            creditsText.rectTransform.position = new Vector2(creditsText.rectTransform.position.x, creditsText.rectTransform.position.y + Y_SCROLL);
            TotalHeightScrolled += Y_SCROLL;
        }
	}

    void Init()
    {
        TotalHeightScrolled = 0;
        RectTransform canvas = gameObject.transform.root as RectTransform;
        float canvasHeight = canvas.sizeDelta.y;
        HeightToScroll = canvasHeight + creditsText.preferredHeight;
        creditsText.rectTransform.position = new Vector2(initialPosition.x, initialPosition.y - canvasHeight);
    }

    public void ShowCredits()
    {
        ShowingCredits = true;
    }

    public void HideCredits()
    {
        ShowingCredits = false;
        Init();
    }

    bool IsFinished
    {
        get
        {
            return TotalHeightScrolled >= HeightToScroll;
        }
    }
    float HeightToScroll { get; set; }
    float TotalHeightScrolled { get; set; }
}
