using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonManager : MonoBehaviour
{

    /// <summary>
    /// The delegate (method signature) that should be used for the function that will be subscribing to the AttackButtonClickedEvent.
    /// </summary>
    /// <see cref="AttackButtonClickedEvent"/>
    /// <param name="values">The values that were selected by the user and should be used to calculate the result of their actions.</param>
    public delegate void AttackActionHandler(int[] values);
    /// <summary>
    /// This event is triggered whenever the user clicks the AttackButton.
    /// </summary>
    public event AttackActionHandler AttackButtonClickedEvent;

    public Sprite[] DefaultSprites;
    public Sprite[] SelectedSprites;

    public float VerticalHeightRatio;

    public ActionButton[] ActionButtons;
    public AttackButton AttackButton;

    /// <summary>
    /// A data structure that keeps track of the user's two most recently selected actions. It ensures that the user has a max of two action selected.
    /// </summary>
    internal ActionSequence buttonSequence { get; set; }

    void Start()
    {
        SetButtons();
        AttackButton.IsEnabled = false;
        InitializedButtonPositions = false;
        //InitActionButtons(new int[] { 1, 2, 5 });     //For Testing
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Selects or Deselects the clicked button.
    /// </summary>
    /// <param name="button"></param>
    public void ActionButtonClicked(ActionButton button)
    {
        if (!button.IsSelected)
        {
            button.IsSelected = true;
            buttonSequence.Add(button);
        }
        else
        {
            button.IsSelected = false;
            buttonSequence.Remove(button);
        }

        if (buttonSequence.Count > 1)
        {
            AttackButton.IsEnabled = true;
        }
        else if (AttackButton.IsEnabled)
        {
            AttackButton.IsEnabled = false;
        }
    }
    /// <summary>
    /// Deselects all of the action buttons and triggers the 'AttackButtonClickedEvent'.
    /// <remarks>
    /// This function gets called by the UnityButton OnClick event
    /// </remarks>
    /// </summary>
    /// <param name="button"></param>
    public void AttackButtonClicked(AttackButton button)
    {
        if (AttackButton.IsEnabled)
        {
            AttackButton.IsEnabled = false;
            ResetActionButtons();

            if (AttackButtonClickedEvent != null)
            {
                AttackButtonClickedEvent(GetValues());
            }

            buttonSequence = new ActionSequence();
        }
    }

    public float ButtonHeight
    {
        get
        {
            RectTransform canvasSize = gameObject.transform.root.transform as RectTransform;
            return VerticalHeightRatio * canvasSize.sizeDelta.y;
        }
    }

    /// <summary>
    /// Initializes the action buttons to represent the numbers contained within the 'values' parameter. The array must contain three values.
    /// </summary>
    /// <param name="values">An array of the values that each button should represent.</param>
    internal void InitActionButtons(int[] values)
    {
        if (values.Length == 3)
        {
            buttonSequence = new ActionSequence();

            for (int i = 0; i < ActionButtons.Length; i++)
            {
                int spriteIndex = values[i] - 1;
                ActionButton ab = ActionButtons[i];
                ab.IsSelected = false;
                ab.InitActionButton(ButtonHeight, DefaultSprites[spriteIndex], SelectedSprites[spriteIndex], values[i]);
            }

            AttackButton.InitAttackButton(ButtonHeight);

            if (!InitializedButtonPositions)
            {
                //Calculations for positioning.
                RectTransform canvas = gameObject.transform.root.transform as RectTransform;
                float attackButtonOffset = .025f * canvas.sizeDelta.x;
                float bottomMargin = .025f * canvas.sizeDelta.y;
                float totalWidthOfButtons = ActionButtons.Length * ActionButtons[0].Width + attackButtonOffset + AttackButton.Width;
                float remainingCanvasWidth = canvas.sizeDelta.x - totalWidthOfButtons;

                //Set positions.
                float totalOffset = remainingCanvasWidth / 2;
                foreach (ActionButton button in ActionButtons)
                {
                    button.SetAbsolutePosition(totalOffset, bottomMargin);
                    totalOffset += button.Width;
                }
                totalOffset += attackButtonOffset;
                AttackButton.SetAbsolutePosition(totalOffset, bottomMargin);

                InitializedButtonPositions = true;
            }
        }
        else
        {
            throw new UnityException(
                string.Format("Invalid array length. There exist three action buttons so 'values' int[] parameter Length must equal 3. Length = {0}", values.Length));
        }
    }

    /// <summary>
    /// Searches for the Action and Attack Button(s) within the game object's children and saves them for ongoing reference.
    /// </summary>
    void SetButtons()
    {
        ActionButtons = gameObject.GetComponentsInChildren<ActionButton>();
        AttackButton = gameObject.GetComponentInChildren<AttackButton>();
    }

    /// <summary>
    /// Returns the values of the buttons currently selected by the user.
    /// </summary>
    /// <returns></returns>
    int[] GetValues()
    {
        int[] values;

        if (buttonSequence.Queue.Length > 0)
        {
            values = new int[buttonSequence.Count];

            //Foreach ActionButton in the sequence.
            for (int i = 0; i < buttonSequence.Count; i++)
            {
                //Add the value that the button represents to the return array.
                values[i] = buttonSequence.Queue[i].Value;
            }
        }
        else
        {
            values = null;
        }

        return values;
    }

    void ResetActionButtons()
    {
        foreach (ActionButton b in ActionButtons)
        {
            b.IsSelected = false;
        }
    }

    bool InitializedButtonPositions { get; set; }
}

/// <summary>
/// Queue like class that stores the two most recently added objects.
/// </summary>
public class ActionSequence
{
    public ActionSequence()
    {
        Count = 0;
        Queue = new ActionButton[2];
    }

    /// <summary>
    /// Adds the parameter object to the action sequence.
    /// </summary>
    /// <remarks>
    /// The object should have its state altered before being added to the Queue.
    /// </remarks>
    /// <param name="action"></param>
    public void Add(ActionButton action)
    {
        if (Count > 1)
        {
            Queue[0].IsSelected = false;

            Remove(Queue[0]);
            Add(action);
        }
        else
        {
            action.IsSelected = true;
            bool done = false;
            for (int i = 0; i < Queue.Length && !done; i++)
            {
                if (Queue[i] == null)
                {
                    Queue[i] = action;
                    Count++;
                    done = true;
                }
            }
        }
    }

    /// <summary>
    /// Removes the parameter object from the action sequence.
    /// </summary>
    /// <remarks>
    /// The object should have its state altered before it is removed from the queue.
    /// </remarks>
    /// <param name="action"></param>
    public void Remove(ActionButton action)
    {
        if (Count > 0)
        {
            if (Queue[0].Equals(action))
            {
                Queue[0] = Queue[1];
            }
            Queue[1] = null;
            Count--;
        }
    }

    public int Count { get; private set; }
    internal ActionButton[] Queue { get; private set; }
}