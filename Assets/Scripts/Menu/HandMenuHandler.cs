using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenuHandler : MonoBehaviour
{

    [SerializeField] private GameObject Placer;
    [SerializeField] private PlaceMenuHandler PlaceMenuHandler;
    private QuboidPlacer CuboidPlacer;
    private Placables? ActivePlacer;

    void OnEnable()
    {
        this.CuboidPlacer = Placer.GetComponent<QuboidPlacer>();

    }

    public void SetCuboidPlacerEnabled(bool enabled)
    {
        this.CuboidPlacer.enabled = enabled;
        if (enabled)
        {
            this.ActivePlacer = Placables.QUBOID;
        }
        else
        {
            this.ActivePlacer = null;
        }

    }

    public void SetPausePlacers(bool paused)
    {
        switch (this.ActivePlacer)
        {
            case Placables.QUBOID:
                this.CuboidPlacer.SetPausePlacer(paused);
                break;
            case Placables.PYRAMID:
                break;
            case Placables.SPHERE:
                break;
            case Placables.CYLINDER:
                break;
        }
    }

    public void TurnOffPlacer(int figureTypeIntRepresented)
    {
        Placables figureType = (Placables)figureTypeIntRepresented;
        if (this.ActivePlacer == figureType)
        {
            this.ActivePlacer = null;
            switch (figureType)
            {
                case Placables.QUBOID:
                    this.PlaceMenuHandler.SetCuboidToggle(false);
                    this.CuboidPlacer.enabled = false;
                    break;
                case Placables.PYRAMID:
                    break;
                case Placables.SPHERE:
                    break;
                case Placables.CYLINDER:
                    break;
            }
        } else
        {
            Debug.LogError("Illegal state. Tried to turn off placer that is currently not selected");
        }

    }
}
