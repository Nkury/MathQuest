using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MovementTutorial : MonoBehaviour {

    public Key[] Keys;
    public Image Direction;
    public Image Background;

	// Use this for initialization
	void Start ()
    {
        Background.gameObject.SetActive(false);
        if (Keys == null || Direction == null || Background == null)
        {
            throw new UnityException("Missing required parameter(s). Check the script parameters in Unity.");
        }

        audioSource = gameObject.GetComponent<AudioSource>();
        StartCoroutine(InitMovementTutorial());
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    internal IEnumerator InitMovementTutorial()
    {
        Background.gameObject.SetActive(true);
        foreach (Key key in Keys)
        {
            SelectKey(key, true);
            yield return new WaitForSeconds(.5f);
            SelectKey(key, false);
            yield return new WaitForSeconds(.5f);
            SelectKey(key, true);
            yield return new WaitForSeconds(.5f);
            SelectKey(key, false);
            yield return new WaitForSeconds(.5f);
        }
        Background.gameObject.SetActive(false);
    }

    private void SelectKey(Key key, bool value)
    {
        key.IsSelected = value;
        SetDirection(key);
    }

    private void SetDirection(Key key)
    {
        if (key.IsSelected)
        {
            Direction.sprite = key.DirectionSprite;
            Direction.color = Key.ActiveArrowColor;
            audioSource.Play();
        }
        else
        {
            Direction.color = Key.DefaultColor;
        }
    }

    private AudioSource audioSource;
}
