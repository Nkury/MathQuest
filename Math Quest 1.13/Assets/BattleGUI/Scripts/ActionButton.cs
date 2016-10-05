using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System;

public class ActionButton : MonoBehaviour {

    public Button Button;
    public int Value { get; internal set; }
    internal Sprite SelectedSprite { get; set; }
    
	void Start ()
    {
        DefaultColor = new Color(.85f, .85f, .85f, 1);
        IsSelected = false;
	}

    /// <summary>
    /// Initializes the button with a size dependent on the total canvas size.
    /// </summary>
    /// <param name="withButtonHeight">Determines the size of the button relative to the canvas height.</param>
    /// <param name="defaultSprite">The sprite that is shown before any user interaction.</param>
    /// <param name="overrideSprite">The sprite that is shown when a button is selected.</param>
    /// <param name="value">The value that the button represents.</param>
    /// <returns>The button's RectTransform which can be used to calculate relative positions to this button.</returns>
    internal void InitActionButton(float withButtonHeight, Sprite defaultSprite, Sprite overrideSprite, int value)
    {
        Height = withButtonHeight;
        RectTransform transform = Button.transform as RectTransform;
        transform.sizeDelta = new Vector2(withButtonHeight * WidthToHeightRatio, withButtonHeight);

        InitActionButton(defaultSprite, overrideSprite, value);
    }

    /// <summary>
    /// Initializes the button with the specified sprites and value.
    /// </summary>
    /// <param name="defaultSprite">The sprite that is shown before any user interaction.</param>
    /// <param name="overrideSprite">The sprite that is shown when a button is selected.</param>
    /// <param name="value">The value that the button represents.</param>
    internal void InitActionButton(Sprite defaultSprite, Sprite overrideSprite, int value)
    {
        Button.image.sprite = defaultSprite;
        SelectedSprite = overrideSprite;
        Value = value;                
    }

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            isSelected = value;
            if (isSelected)
            {
                Select();
            }
            else
            {
                Deselect();
            }
        }
    }

    public void Select()
    {
        Button.image.color = Color.white;
        Button.image.overrideSprite = SelectedSprite;
    }

    public void Deselect()
    {
        Button.image.color = DefaultColor;
        Button.image.overrideSprite = null;
    }
    internal void SetAbsolutePosition(float x, float y)
    {
        Button.transform.position = new Vector2(x, y);
    }

    internal static float HeightToWidthRatio
    {
        get
        {
            return 50f / 160f;
        }
    }

    internal static float WidthToHeightRatio
    {
        get
        {
            return 160f / 50f;
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

    void Disable()
    {
        IsSelected = false;
        Button.image.color = new Color(1f, 1f, 1f, .5f);
    }

    static Color DefaultColor { get; set; }

    bool isSelected = false;
}