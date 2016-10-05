using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class PostBattleMenu : MonoBehaviour {

    public GameObject[] Stars;
    public GameObject FireworksParticle;
    public RectTransform ResultsPanel;
    public RectTransform StarPanel;
    public RectTransform Medal;
    public RectTransform TextPanel;
    public Sprite overrideMedalSprite;

	// Use this for initialization
	void Start ()
    {
        FireworksParticle.SetActive(false);
        gameObject.SetActive(false);
        //InitPostBattleMenuWith(300, 400, 3, new int[] { 1, 2, 3, 4, 5, 6 }); //For testing.
	}

    /// <summary>
    /// Initializes the post battle menu with the parameter dimensions and player results.
    /// </summary>
    /// <param name="Width">The width with which the panel should be rendered.</param>
    /// <param name="Height">THe height with which the panel should be rendered.</param>
    /// <param name="NumberOfStars"></param>
    /// <param name="PlayerActions"></param>
    public void InitPostBattleMenuWith(float Width, float Height, int NumberOfStars, int[] PlayerActions)
    {
        ResultsPanel.sizeDelta = new Vector2(Width, Height);
        InitPostBattleMenuWith(NumberOfStars, PlayerActions);
    }

    /// <summary>
    /// Renders the post battle assessment menu based on the function parameters.
    /// </summary>
    /// <param name="NumberOfStars">The number of stars to render. Used to determine whether or not the medal renders.</param>
    /// <param name="PlayerActions">The actions (values of attacks) that the player executed. Used to detail the player's evaluation.</param>
    public void InitPostBattleMenuWith(int NumberOfStars, int[] PlayerActions)
    {
        this.NumberOfStars = NumberOfStars;
        this.PlayerActions = PlayerActions;

        float panelWidth = ResultsPanel.sizeDelta.x;

        //Set the size of the star panel using runtime values.
        StarPanel.sizeDelta = new Vector2(panelWidth, StarHeight);
        float totalHeightOffset = StarPanel.transform.position.y;
        totalHeightOffset = InitRow(StarHeight, totalHeightOffset, ref StarPanel);

        //We each star's position and size using the runtime absolute values.
        RectTransform star1 = Stars[0].GetComponent<RectTransform>();
        star1.sizeDelta = new Vector2(StarHeight, StarHeight);
        star1.position = new Vector3(star1.position.x - star1.sizeDelta.x - StarMargin, star1.position.y, star1.position.z);

        RectTransform star2 = Stars[1].GetComponent<RectTransform>();
        star2.sizeDelta = new Vector2(StarHeight, StarHeight);

        RectTransform star3 = Stars[2].GetComponent<RectTransform>();
        star3.sizeDelta = new Vector2(StarHeight, StarHeight);
        star3.position = new Vector3(star3.position.x + star3.sizeDelta.x + StarMargin, star3.position.y, star3.position.z);

        //We set the proper star color based on the player's performance.
        for (int i = 0; i < NumberOfStars; i++)
        {
            Stars[i].GetComponent<Image>().color = StarColor;
        }

        //We set the size of the medal based on runtime values.
        totalHeightOffset = InitRow(MedalHeight, totalHeightOffset, ref Medal);

        //If the player achieved mastery.
        if (NumberOfStars == 3)
        {
            Image medalImage = Medal.gameObject.GetComponent<Image>();
            medalImage.color = Color.white;
            medalImage.overrideSprite = overrideMedalSprite;
            QuestDelegate.masteryQuestUpdated(1);
        }

        //We set the size of the text area that will display the user's actions.
        totalHeightOffset = InitRow(TotalTextHeight, totalHeightOffset, ref TextPanel);

        Text ResultText = TextPanel.GetComponent<Text>();
        ResultText.text = string.Empty;

        int[] results = new int[PlayerActions.Length / 2];
        int total = 0;

        for (int i = 0, j = 0; i < PlayerActions.Length; i += 2, j++)
        {
            int x = PlayerActions[i];
            int y = PlayerActions[i + 1];
            total += (x + y);
            results[j] = (x+y);

            ResultText.text += string.Format("{0} + {1} = {2}\n", x, y, x + y);
        }

        if (results.Length > 1)
        {
            string summary = string.Empty;
            for (int i = 0; i < results.Length; i++)
            {
                if (i != results.Length - 1)
                {
                    summary += string.Format("{0} + ", results[i]);
                }
                else
                {
                    summary += string.Format("{0} = ", results[i]);
                }
            }
            summary += total;
            ResultText.text += summary;

        }
        gameObject.SetActive(true);
        FireworksParticle.SetActive(true);
    }

    /// <summary>
    /// </summary>
    /// <param name="Index"></param>
    /// <param name="PreviousElementHeight"></param>
    /// <param name="CanvasObject"></param>
    float InitRow(float WithHeight, float At_Y_Position, ref RectTransform CanvasObject)
    {
        CanvasObject.sizeDelta = new Vector2(ResultsPanel.sizeDelta.x, WithHeight);
        CanvasObject.transform.position = new Vector2(CanvasObject.transform.position.x, At_Y_Position - Margin);

        return At_Y_Position - Margin - WithHeight;
    }

    int NumberOfStars { get; set; }
    int[] PlayerActions { get; set; }

    bool ShouldRenderTrophy
    {
        get
        {
            return NumberOfStars == 3;
        }
    }

    int NumberOfTextRows
    {
        get
        {
            return PlayerActions == null ? 0 : PlayerActions.Length + 1;
        }
    }

    float Margin
    {
        get
        {
            return .05f * ResultsPanel.sizeDelta.y;
        }
    }
    float StarHeight
    {
        get
        {
            return .15f * ResultsPanel.sizeDelta.y;
        }
    }

    float StarMargin
    {
        get
        {
            return .05f * ResultsPanel.sizeDelta.x;
        }
    }

    float MedalHeight
    {
        get
        {
            return .2f * ResultsPanel.sizeDelta.y;
        }
    }
    float TotalTextHeight
    {
        get
        {
            return .45f * ResultsPanel.sizeDelta.y;
        }
    }

    float TextHeight
    {
        get
        {
            return TotalTextHeight / ((PlayerActions.Length / 2) + 1);
        }
    }

    internal static Color StarColor = new Color(1f, 247f / 255f, 0f, 1);
    internal static Color DisabledStarColor = new Color(188f / 255f, 188f / 255f, 188f / 255f, 125f / 255f);
    internal static Color DisabledMedalColor = new Color(1f, 1f, 1f, 50f / 255f);
}
