using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class Key : MonoBehaviour {

    public Sprite OverrideSprite;
    public Sprite DirectionSprite;

    void Start()
    {
        image = gameObject.GetComponent<Image>();

        if (image == null || OverrideSprite == null || DirectionSprite == null)
        {
            throw new UnityException("Missing a required parameter. Check the script parameters in Unity.");
        }
    }

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        internal set
        {
            isSelected = !isSelected;
            if (image != null)
            {
                UpdateSprite();
            }                        
        }
    }

    public static Color DefaultColor
    {
        get
        {
            return defaultColor;
        }
    }

    public static Color ActiveArrowColor
    {
        get
        {
            return activeArrowColor;
        }
    }

    private void UpdateSprite()
    {
        if (IsSelected)
        {
            image.color = Color.white;
            image.overrideSprite = OverrideSprite;
        }
        else
        {
            image.color = defaultColor;
            image.overrideSprite = null;
        }
    }

    private bool isSelected = false;
    private Image image;
    private static Color defaultColor = new Color(159f/255f, 159f/255f, 159f/255f, 1f);
    private static Color activeArrowColor = new Color(0f, 238f / 255f, 51f / 255f, 1f);
}

public enum MovementKeys
{
    W,
    A,
    S,
    D,
    Q

}

public static class MovementKeyExtension
{
    public static int ToIndex(this MovementKeys key)
    {
        return (int)key;
    }
}
