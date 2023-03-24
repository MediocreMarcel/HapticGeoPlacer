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
    //Placer
    [SerializeField] private GameObject Placer;

    private QuboidPlacer CuboidPlacer;
    private SpherePlacer SpherePlacer;
    private CylinderPlacer CylinderPlacer;
    private PyramidPlacer PyramidPlacer;

    private Placables? ActivePlacer;


    private void Start()
    {
        //Get All Placers
        this.CuboidPlacer = Placer.GetComponent<QuboidPlacer>();
        this.SpherePlacer = Placer.GetComponent<SpherePlacer>();
        this.CylinderPlacer = Placer.GetComponent<CylinderPlacer>();
        this.PyramidPlacer = Placer.GetComponent<PyramidPlacer>();
    }

    /// <summary>
    /// Handels Toggle envents of the Quboid place button
    /// </summary>
    /// <param name="toggle">Toggle state of event</param>
    public void OnQuboidToggle(bool toggle)
    {
        if (toggle)
        {
            this.SetSphereToggle(false);
            this.SetPyramidToggle(false);
            this.SetCylinderToggle(false);
        }
        this.SetPlacerEnabled(Placables.QUBOID, toggle);
    }

    /// <summary>
    /// Handels Toggle envents of the Sphere place button
    /// </summary>
    /// <param name="toggle">Toggle state of event</param>
    public void OnSphereToggle(bool toggle)
    {
        if (toggle)
        {
            this.SetQuboidToggle(false);
            this.SetPyramidToggle(false);
            this.SetCylinderToggle(false);
        }
        this.SetPlacerEnabled(Placables.SPHERE, toggle);
    }

    /// <summary>
    /// Handels Toggle envents of the Pyramid place button
    /// </summary>
    /// <param name="toggle">Toggle state of event</param>
    public void OnPyramidToggle(bool toggle)
    {
        if (toggle)
        {
            this.placeSphereToggleButtonInteractable.IsToggled = false;
            this.SetQuboidToggle(false);
            this.SetCylinderToggle(false);
        }
        this.SetPlacerEnabled(Placables.PYRAMID, toggle);
    }

    /// <summary>
    /// Handels Toggle envents of the Cylinder place button
    /// </summary>
    /// <param name="toggle">Toggle state of event</param>
    public void OnCylinderToggle(bool toggle)
    {
        if (toggle)
        {
            this.SetSphereToggle(false);
            this.SetPyramidToggle(false);
            this.SetQuboidToggle(false);
        }
        this.SetPlacerEnabled(Placables.CYLINDER, toggle);
    }

    /// <summary>
    /// Sets the state of the quboid toggle button
    /// </summary>
    /// <param name="isToggled">bool indication if toggle button is active</param>
    public void SetQuboidToggle(bool isToggled)
    {
        this.placeQuboidToggleButtonInteractable.IsToggled = isToggled;
    }

    /// <summary>
    /// Sets the state of the sphere toggle button
    /// </summary>
    /// <param name="isToggled">bool indication if toggle button is active</param>
    public void SetSphereToggle(bool isToggled)
    {
        this.placeSphereToggleButtonInteractable.IsToggled = isToggled;
    }

    /// <summary>
    /// Sets the state of the cylinder toggle button
    /// </summary>
    /// <param name="isToggled">bool indication if toggle button is active</param>
    public void SetCylinderToggle(bool isToggled)
    {
        this.placeCylinderToggleButtonInteractable.IsToggled = isToggled;
    }

    /// <summary>
    /// Sets the state of the pyramid toggle button
    /// </summary>
    /// <param name="isToggled">bool indication if toggle button is active</param>
    public void SetPyramidToggle(bool isToggled)
    {
        this.placePyramidToggleButtonInteractable.IsToggled = isToggled;
    }

    /// <summary>
    /// Changes the enabled status of the given placer type to the given enabled value. 
    /// This method also changes the active placer variable according to the selected placer.
    /// </summary>
    /// <param name="placerType">Type of placer that should be dis-/enabled</param>
    /// <param name="enabled">bool indicating if placer should be enabled</param>
    public void SetPlacerEnabled(Placables placerType, bool enabled)
    {
        switch (placerType)
        {
            case Placables.QUBOID:
                this.CuboidPlacer.enabled = enabled;
                break;
            case Placables.PYRAMID:
                this.PyramidPlacer.enabled = enabled;
                break;
            case Placables.SPHERE:
                this.SpherePlacer.enabled = enabled;
                break;
            case Placables.CYLINDER:
                this.CylinderPlacer.enabled = enabled;
                break;
        }

        if (enabled)
        {
            this.ActivePlacer = placerType;
        }
        else
        {
            this.ActivePlacer = null;
        }
    }

    /// <summary>
    /// Sets the pause status of a placer.
    /// The pause status will not delete allready set start points.
    /// </summary>
    /// <param name="paused">bool indicating if a placer should be paused</param>
    public void SetPausePlacers(bool paused)
    {
        switch (this.ActivePlacer)
        {
            case Placables.QUBOID:
                this.CuboidPlacer.SetPausePlacer(paused);
                break;
            case Placables.PYRAMID:
                this.PyramidPlacer.SetPausePlacer(paused);
                break;
            case Placables.SPHERE:
                this.SpherePlacer.SetPausePlacer(paused);
                break;
            case Placables.CYLINDER:
                this.CylinderPlacer.SetPausePlacer(paused);
                break;
        }
    }

    /// <summary>
    /// Sets enabled for all placers to false
    /// </summary>
    public void TurnOffAllPlacers()
    {
        this.SetQuboidToggle(false);
        this.CuboidPlacer.enabled = false;

        this.SetPyramidToggle(false);
        this.PyramidPlacer.enabled = false;

        this.SetSphereToggle(false);
        this.SpherePlacer.enabled = false;

        this.SetCylinderToggle(false);
        this.CylinderPlacer.enabled = false;
    }

    /// <summary>
    /// Sets enabled false to the given placer. Also resets the Toggle button.
    /// </summary>
    /// <param name="figureTypeIntRepresented">Int representation of an entry of the Placables Enum</param>
    public void TurnOffPlacer(int figureTypeIntRepresented)
    {
        Placables figureType = (Placables)figureTypeIntRepresented;
        if (this.ActivePlacer == figureType)
        {
            this.ActivePlacer = null;
            switch (figureType)
            {
                case Placables.QUBOID:
                    this.SetQuboidToggle(false);
                    this.CuboidPlacer.enabled = false;
                    break;
                case Placables.PYRAMID:
                    this.SetPyramidToggle(false);
                    this.PyramidPlacer.enabled = false;
                    break;
                case Placables.SPHERE:
                    this.SetSphereToggle(false);
                    this.SpherePlacer.enabled = false;
                    break;
                case Placables.CYLINDER:
                    this.SetCylinderToggle(false);
                    this.CylinderPlacer.enabled = false;
                    break;
            }
        }
        else
        {
            Debug.LogError("Illegal state. Tried to turn off placer that is currently not selected");
        }

    }

    /// <summary>
    /// Gently closes the Menu by deactivating all toggle button functions
    /// </summary>
    public void CloseMenu()
    {
        this.placeSphereToggleButtonInteractable.IsToggled = false;
        this.placeQuboidToggleButtonInteractable.IsToggled = false;
        this.placeCylinderToggleButtonInteractable.IsToggled = false;
        this.placePyramidToggleButtonInteractable.IsToggled = false;
        gameObject.SetActive(false);
    }
}
