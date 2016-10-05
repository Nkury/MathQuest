using UnityEngine;
using System.Collections;

public class MainMenuButtons : MonoBehaviour {

    public string SceneToLoad;
    public Credits credits;

    bool showingCredits = false;
    public void PlayButtonClicked ()
    {
        if (string.IsNullOrEmpty(SceneToLoad))
        {
            Debug.Log("You did not specify a scene to load!");
        }
        else
        {
            Application.LoadLevel(SceneToLoad);
        }
    }

    public void CreditButtonClicked()
    {
        showingCredits = !showingCredits;
        if (showingCredits)
        {
            credits.ShowCredits();
        }
        else
        {
            credits.HideCredits();
        }
    }
    public void ExitButtonClicked ()
    {
        Debug.Log("Quitting Application.");
        Application.Quit();
    }

    public void toTitleClicked()
    {
        Application.LoadLevel("title screen");
    }

}
