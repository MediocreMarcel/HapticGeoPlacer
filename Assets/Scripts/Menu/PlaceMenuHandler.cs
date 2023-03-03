using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using UnityEngine;

public class PlaceMenuHandler : MonoBehaviour
{
    //Buttons
    [SerializeField] private Interactable placeQuboidToggleButtonInteractable;
    [SerializeField] private Interactable placeSphereToggleButtonInteractable;
    [SerializeField] private Interactable placePyramidToggleButtonInteractable;
    [SerializeField] private Interactable placeCylinderToggleButtonInteractable;

    //HandMenuHandler
    [SerializeField] private HandMenuHandler handMenuHandler;
    //Solver for hand
    [SerializeField] private HandConstraintPalmUp HandConstraintPalmUp;


    public void OnEnable()
    {
        Debug.Log(HandConstraintPalmUp.Handedness);
        if (HandConstraintPalmUp.Handedness == Handedness.Right && transform.localPosition.x > 0)
        {
            Vector3 rightHandedPosition = transform.localPosition;
            rightHandedPosition.x = -rightHandedPosition.x;
            Debug.Log(rightHandedPosition);
            transform.localPosition = rightHandedPosition;
        }
    }

    public void OnQuboidToggle(bool toggle)
    {
        if (toggle)
        {
        this.placeSphereToggleButtonInteractable.IsToggled = false;
        this.placePyramidToggleButtonInteractable.IsToggled = false;
        this.placeCylinderToggleButtonInteractable.IsToggled = false;
        }
        this.handMenuHandler.SetCuboidPlacerEnabled(toggle);
    }

    public void OnSphereToggle(bool toggle)
    {
        this.placeQuboidToggleButtonInteractable.IsToggled = false;
        this.placePyramidToggleButtonInteractable.IsToggled = false;
        this.placeCylinderToggleButtonInteractable.IsToggled = false;
    }

    public void OnPyramidToggle(bool toggle)
    {
        this.placeSphereToggleButtonInteractable.IsToggled = false;
        this.placeQuboidToggleButtonInteractable.IsToggled = false;
        this.placeCylinderToggleButtonInteractable.IsToggled = false;
    }

    public void OnCylinderToggle(bool toggle)
    {
        this.placeSphereToggleButtonInteractable.IsToggled = false;
        this.placePyramidToggleButtonInteractable.IsToggled = false;
        this.placeQuboidToggleButtonInteractable.IsToggled = false;
    }

    public void SetCuboidToggle(bool isToggled)
    {
        this.placeQuboidToggleButtonInteractable.IsToggled = isToggled;
    }
}
