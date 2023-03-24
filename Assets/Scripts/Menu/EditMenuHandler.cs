using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using UnityEngine;

public class EditMenuHandler : MonoBehaviour
{
    [SerializeField] private Interactable SizeToggleButton;

    /// <summary>
    /// Method handels the Edit Size toggle Button and activates/deactivates the Bound control on placed objects.
    /// </summary>
    /// <param name="isToggled">state of the button as boolean</param>
    public void OnEditSizeToggle(bool isToggled)
    {
        GameObject[] Figures = GameObject.FindGameObjectsWithTag("PlacedFigure");
        foreach (GameObject Figure in Figures)
        {
            BoundsControl BoundsControl = Figure.GetComponent<BoundsControl>();
            BoundsControl.enabled = isToggled;
        }
    }

    private void OnDisable()
    {
    }
    /// <summary>
    /// Gently closes the Menu by deactivating all toggle button functions
    /// </summary>
    public void CloseMenu()
    {
        this.SizeToggleButton.IsToggled = false;
        this.OnEditSizeToggle(false);
        gameObject.SetActive(false);
    }

}
