using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public enum FigurColors
{
    RED, BLUE, GREEN, YELLOW
}

public class ColorMenuHandler : MonoBehaviour
{

    [SerializeField] private Interactable RedToggleButtonInteractable;
    [SerializeField] private Interactable BlueToggleButtonInteractable;
    [SerializeField] private Interactable GreenToggleButtonInteractable;
    [SerializeField] private Interactable YellowToggleButtonInteractable;
    private FigurColors? SelectedColor;

    private void OnEnable()
    {
        this.changeColorChangerStatus(true);
    }


    /// <summary>
    /// Handels the events of the Yellow Toggle Button
    /// </summary>
    /// <param name="toggle">toggle status of button event</param>
    public void onYellowToggle(bool toggle)
    {
        if (toggle)
        {
            this.RedToggleButtonInteractable.IsToggled = false;
            this.BlueToggleButtonInteractable.IsToggled = false;
            this.GreenToggleButtonInteractable.IsToggled = false;
            this.SelectedColor = FigurColors.YELLOW;
        }
    }

    /// <summary>
    /// Handels the events of the Red Toggle Button
    /// </summary>
    /// <param name="toggle">toggle status of button event</param>
    public void onRedToggle(bool toggle)
    {
        if (toggle)
        {
            this.YellowToggleButtonInteractable.IsToggled = false;
            this.BlueToggleButtonInteractable.IsToggled = false;
            this.GreenToggleButtonInteractable.IsToggled = false;
            this.SelectedColor = FigurColors.RED;
        }
    }

    /// <summary>
    /// Handels the events of the Blue Toggle Button
    /// </summary>
    /// <param name="toggle">toggle status of button event</param>
    public void onBlueToggle(bool toggle)
    {
        if (toggle)
        {
            this.RedToggleButtonInteractable.IsToggled = false;
            this.YellowToggleButtonInteractable.IsToggled = false;
            this.GreenToggleButtonInteractable.IsToggled = false;
            this.SelectedColor = FigurColors.BLUE;
        }
    }

    /// <summary>
    /// Handels the events of the Green Toggle Button
    /// </summary>
    /// <param name="toggle">toggle status of button event</param>
    public void onGreenToggle(bool toggle)
    {
        if (toggle)
        {
            this.RedToggleButtonInteractable.IsToggled = false;
            this.BlueToggleButtonInteractable.IsToggled = false;
            this.YellowToggleButtonInteractable.IsToggled = false;
            this.SelectedColor = FigurColors.GREEN;
        }
    }

    /// <summary>
    /// Gets a Color object of the currently selected color
    /// </summary>
    /// <returns>Color object if anything is selected, null if nothing is selected</returns>
    public Color? GetSelectedColor()
    {
        switch (this.SelectedColor)
        {
            case FigurColors.RED:
                return new Color(0.353f, 0.0f, 0.0f);
            case FigurColors.BLUE:
                return new Color(0.066f, 0.122f, 0.412f);
            case FigurColors.GREEN:
                return new Color(0.0f, 0.686f, 0.0f);
            case FigurColors.YELLOW:
                return new Color(0.75f, 0.75f, 0.0f);
            default:
                return null;
        }
    }

    /// <summary>
    /// Gently closes the Menu by deactivating all toggle button functions
    /// </summary>
    public void CloseMenu()
    {
        this.RedToggleButtonInteractable.IsToggled = false;
        this.YellowToggleButtonInteractable.IsToggled = false;
        this.GreenToggleButtonInteractable.IsToggled = false;
        this.BlueToggleButtonInteractable.IsToggled = false;
        this.SelectedColor = null;
        this.changeColorChangerStatus(false);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Changes the status of the Color Changer Script on any placed figure
    /// </summary>
    /// <param name="active">enabled status that the script should get</param>
    private void changeColorChangerStatus(bool active)
    {
        GameObject[] Figures = GameObject.FindGameObjectsWithTag("PlacedFigure");
        foreach (GameObject Figure in Figures)
        {
            ColorChanger colorChanger = Figure.GetComponent<ColorChanger>();
            if (active)
            {
                colorChanger.ColorMenuHandler = this;
            }

            colorChanger.enabled = active;
        }
    }
}
