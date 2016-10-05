using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarManager : MonoBehaviour
{

    internal readonly float HP_ICON_OFFSET = 2.5f;

    Image IconObject;
    public Camera CanvasCamera;
    public GameObject PositionRelativeTo;
    public float IconVerticalRatio;
    public int NumberOfVisibleIcons;
    public Sprite DefaultSprite;
    public Sprite OverrideSprite;
    public bool IsLeftToRight;

    // Use this for initialization
    void Start()
    {
        IconObject = gameObject.GetComponentInChildren<Image>();

        //For Testing
        //InitHealthBar(PositionRelativeTo, NumberOfVisibleIcons == 3 ? 3 : 30);
        //if (NumberOfVisibleIcons != 3)
        //{
        //    OverrideSpriteFor(3);
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Renders the health bar of a game world object above the character.
    /// </summary>
    /// <param name="relativeToObject">The game object above which you would like to render the health bar.</param>
    /// <param name="withNumberOfIcons">The number of icons (units of health) that should appear above the object.</param>
    internal void InitHealthBar(GameObject relativeToObject, int withNumberOfIcons)
    {
        ResetHealthBar();
        if (IconObject == null)
            IconObject = gameObject.GetComponentInChildren<Image>();

        NumberOfVisibleIcons = withNumberOfIcons;
        Icons = new Image[NumberOfVisibleIcons];

        // We will need to shift the healthbar upwards if there are more than 10 icons.
        int initialBottomOffset = (withNumberOfIcons - 1) / 10;

        Vector3 objectsCanvasPosition = RectTransformUtility.WorldToScreenPoint(CanvasCamera, relativeToObject.transform.position);

        //Set the health bar's absolute position, above the player's health.
        gameObject.transform.parent.transform.position = new Vector2(
            objectsCanvasPosition.x - HealthBarOffset,
            objectsCanvasPosition.y + (objectsCanvasPosition.y / 2) + (IconSize * initialBottomOffset));

        float totalXOffset = IconObject.rectTransform.position.x;
        float totalYOffset = IconObject.rectTransform.position.y;

        //Set all of the icon's absolute positions.
        for (int i = 0; i < withNumberOfIcons; i++)
        {
            Image clone = Instantiate<Image>(IconObject);
            clone.name = string.Format("Icon({0})", i);
            clone.rectTransform.SetParent(IconObject.transform.parent);
            clone.rectTransform.position = new Vector2(totalXOffset, totalYOffset);
            clone.rectTransform.sizeDelta = new Vector2(IconSize, IconSize);
            clone.sprite = DefaultSprite;
            clone.overrideSprite = null;
            clone.enabled = true;

            Icons[i] = clone;

            //If we have created a multiple of ten icons.
            if ((i + 1) % 10 == 0)
            {
                //Reset the offset.
                totalXOffset = IconObject.rectTransform.position.x;

                //Decrement the Y offset so that icons render beneath the already created icons.
                totalYOffset -= (IconSize + HP_ICON_OFFSET);
            }
            else
            {
                totalXOffset += IconSize + HP_ICON_OFFSET;
            }
        }
    }

    /// <summary>
    /// Calling this function overrides the default sprite with the override sprite that was the Manager was initialized with.
    /// </summary>
    /// <param name="numberOfIcons">The number of icons to override.</param>
    public void OverrideSpriteFor(int numberOfIcons)
    {
        if (numberOfIcons > 0 && TotalIconsOverridden < Capacity)
        {
            int totalIconsToOverride = TotalIconsOverridden + numberOfIcons;
            totalIconsToOverride = totalIconsToOverride > Capacity ? Capacity : totalIconsToOverride;

            int iconsOverriden = 0;
            if (IsLeftToRight)
            {
                for (int i = 0; i < Capacity && iconsOverriden < numberOfIcons; i++)
                {
                    OverrideSpriteOf(ref Icons[i], OverrideSprite);
                    iconsOverriden++;
                }
            }
            else
            {
                for (int i = NumberOfVisibleIcons - TotalIconsOverridden - 1; i >= 0 && iconsOverriden < numberOfIcons; i--)
                {
                    OverrideSpriteOf(ref Icons[i], OverrideSprite);
                    iconsOverriden++;
                }
            }
        }
    }

    /// <summary>
    /// Sets the override sprite of an icon. If you wish to simple remove the icon set the 'withSprite' parameter to null.
    /// </summary>
    /// <param name="image">The icon image that contains the sprite object.</param>
    /// <param name="withSprite">The sprite with which to override the current icon.</param>
    void OverrideSpriteOf(ref Image image, Sprite withSprite)
    {
        if (image != null)
        {
            if (withSprite == null)
            {
                image.enabled = false; ;
            }
            else
            {
                image.overrideSprite = withSprite;
            }

            TotalIconsOverridden++;

        }
    }

    internal void ResetHealthBar()
    {

        if (Icons != null && Icons.Length > 0)
        {
            for (int i = 0; i < Icons.Length; i++)
            {
                Destroy(Icons[i].gameObject);
            }
        }

        Icons = null;
        NumberOfVisibleIcons = 0;
        TotalIconsOverridden = 0;
    }

    /// <summary>
    /// The Length of the entire 
    /// </summary>
    public int Capacity
    {
        get
        {
            return Icons == null ? 0 : Icons.Length;
        }
    }

    /// <summary>
    /// Returns a the absolute icon sized as determined by the IconVerticalRatio.
    /// </summary>
    public float IconSize
    {
        get
        {
            RectTransform canvasSize = gameObject.transform.root as RectTransform;
            return IconVerticalRatio * canvasSize.sizeDelta.y;
        }
    }

    public float HealthBarOffset
    {
        get
        {
            int iconsUsedToCalculateWidth = NumberOfVisibleIcons < 10 ? NumberOfVisibleIcons : 10;
            return (IconSize * iconsUsedToCalculateWidth) / 2;
        }
    }

    internal Image[] Icons { get; private set; }
    internal int TotalIconsOverridden { get; private set; }
}
