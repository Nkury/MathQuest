using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleTutorial : MonoBehaviour {

    public float VerticalRatio;
    public Sprite RightArrowSprite;
    public RectTransform Arrow;
    public RectTransform[] Buttons;
    public RectTransform EnemyHealth;

	// Use this for initialization
	void Start () 
    {
        arrowImage = Arrow.gameObject.GetComponent<Image>();
        arrowImage.enabled = false;

        background = gameObject.GetComponent<Image>();
        canvas = gameObject.transform.root.transform as RectTransform;

        float arrowDimension = VerticalRatio * canvas.sizeDelta.y;
        Arrow.sizeDelta = new Vector2(arrowDimension, arrowDimension);

	    if (Arrow == null || Buttons == null || VerticalRatio <= 0 || VerticalRatio >= 1)
        {
            throw new UnityException("Required parameter(s) are invalid. Verify that objects are assigned and values are valid.");
        }
	}

    public void startTutorial()
    {
        StartCoroutine(InitBattleTutorial());
    }

    internal IEnumerator InitBattleTutorial()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        AudioSource[] audios = mainCamera.GetComponents<AudioSource>();

        background.enabled = true;

        float arrow_x, arrow_y;

        arrow_x = EnemyHealth.position.x - Arrow.sizeDelta.x - (.015f * canvas.sizeDelta.x);
        arrow_y = EnemyHealth.position.y+30 - Arrow.sizeDelta.y;

        Arrow.position = new Vector2(arrow_x, arrow_y);
        arrowImage.overrideSprite = RightArrowSprite;
        arrowImage.enabled = true;
        audios[7].Play();
        yield return new WaitForSeconds(.5f);
        arrowImage.enabled = false;
        yield return new WaitForSeconds(.5f);
        arrowImage.enabled = true;
        audios[7].Play();
        yield return new WaitForSeconds(.5f);
        arrowImage.enabled = false;
        yield return new WaitForSeconds(1f);

        arrowImage.overrideSprite = null;

        foreach(RectTransform button in Buttons)
        {
            arrow_x = button.position.x + (button.sizeDelta.x / 2f) - (Arrow.sizeDelta.x / 2f);
            arrow_y = button.position.y + button.sizeDelta.y + 10;

            Arrow.position = new Vector2(arrow_x, arrow_y);
            arrowImage.enabled = true;
            audios[7].Play();
            yield return new WaitForSeconds(.5f);
            arrowImage.enabled = false;
            yield return new WaitForSeconds(.5f);
            arrowImage.enabled = true;
            audios[7].Play();
            yield return new WaitForSeconds(.5f);
            arrowImage.enabled = false;
            yield return new WaitForSeconds(.5f);
        }
        arrowImage.enabled = false;
        background.enabled = false;
    }

    private Image arrowImage;
    private RectTransform canvas;
    private Image background;
}
