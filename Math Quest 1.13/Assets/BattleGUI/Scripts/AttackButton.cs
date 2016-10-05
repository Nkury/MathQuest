using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Button))]
public class AttackButton : MonoBehaviour {

    public Sprite DisabledSprite;
    public Sprite EnabledSprite;
    public Sprite SelectedSprite;
    public Button Button;
    public Color DisabledColor { get; private set; }

    void Start ()
    {
        DisabledColor = new Color(1f, 1f, 1f, .5f);
        Button = gameObject.GetComponent<Button>();
        Button.image.color = DisabledColor;
	}

    /// <summary>
    /// Initializes the button with an absolute position and a size dependent on the total canvas size.
    /// </summary>
    /// <param name="withButtonHeight">The button's height</param>
    /// <param name="x">The buttons exact x postion using a bottom left pivot.</param>
    /// <param name="y">The buttons exact y position using a bottom left pivot.</param>
    /// <param name="defaultSprite">The sprite that is shown before any user interaction.</param>
    /// <param name="overrideSprite">The sprite that is shown when a button is selected.</param>
    /// <param name="value">The value that the button represents.</param>
    /// <returns>The button's RectTransform which can be used to calculate relative positions to this button.</returns>
    internal RectTransform InitAttackButton(float withButtonHeight, float x, float y)
    {
        RectTransform transform = InitAttackButton(withButtonHeight);
        transform.position = new Vector2(x, y);

        return transform;
    }

    /// <summary>
    /// Initializes the button with a size dependent on the total canvas size.
    /// </summary>
    /// <param name="withButtonHeight">Determines the size of the button relative to the canvas height.</param>
    /// <param name="defaultSprite">The sprite that is shown before any user interaction.</param>
    /// <param name="overrideSprite">The sprite that is shown when a button is selected.</param>
    /// <param name="value">The value that the button represents.</param>
    /// <returns>The button's RectTransform which can be used to calculate relative positions to this button.</returns>
    internal RectTransform InitAttackButton(float withButtonHeight)
    {
        Height = withButtonHeight;
        RectTransform transform = Button.transform as RectTransform;
        transform.sizeDelta = new Vector2(withButtonHeight * WidthToHeightRatio, withButtonHeight);

        return transform;
    }

    internal static float WidthToHeightRatio
    {
        get
        {
            return 100f / 50f;
        }
    }

    internal static float HeightToWidthRatio
    {
        get
        {
            return 50f / 100f;
        }
    }

    internal float Height { get; private set; }
    internal float Width
    {
        get
        {
            return Height * WidthToHeightRatio;
        }
    }

    internal void SetAbsolutePosition(float x, float y)
    {
        Button.transform.position = new Vector2(x, y);
    }

    public bool IsEnabled
    {
        get
        {
            return isEnabled;
        }
        set
        {
            isEnabled = value;
            if (isEnabled)
            {
                Button.transition = Selectable.Transition.SpriteSwap;
                Button.image.sprite = EnabledSprite;
                SpriteState sprites = new SpriteState();
                sprites.pressedSprite = SelectedSprite;
                Button.spriteState = sprites;
                Button.image.color = Color.white;
            }
            else
            {
                Button.image.color = DisabledColor;
                Button.image.sprite = DisabledSprite;
                Button.transition = Selectable.Transition.None;
            }
        }
    }

    private bool isEnabled = false;
}
