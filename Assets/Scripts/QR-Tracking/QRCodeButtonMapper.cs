// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// Modified by Marcel Heda, HTW Berlin

using Microsoft.MixedReality.QR;
using Microsoft.MixedReality.SampleQRCodes;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

// ==== Beginn Eigenanteil ==== /
enum MenuState
{
    MainMenu,
    PlaceMenu,
    EditMenu
}
// ==== Ende Eigenanteil ==== /

public class QRCodeButtonMapper : MonoBehaviour
{
    // ==== Beginn Eigenanteil ==== /
    public GameObject MenuWrapper;
    public GameObject StartCube;

    private bool isTrackingEnabled = false;
    private MenuState MenuState = MenuState.MainMenu;
    // ==== Ende Eigenanteil ==== /
    private bool clearExisting = false;
    struct ActionData
    {
        public enum Type
        {
            Added,
            Updated,
            Removed
        };
        public Type type;
        public Microsoft.MixedReality.QR.QRCode qrCode;

        public ActionData(Type type, Microsoft.MixedReality.QR.QRCode qRCode) : this()
        {
            this.type = type;
            qrCode = qRCode;
        }
    }

    private Queue<ActionData> pendingActions;

    // Use this for initialization
    void Start()
    {
        Debug.Log("QRCodesVisualizer start");

        QRCodesManager.Instance.QRCodesTrackingStateChanged += Instance_QRCodesTrackingStateChanged;
        QRCodesManager.Instance.QRCodeAdded += Instance_QRCodeAdded;
        QRCodesManager.Instance.QRCodeUpdated += Instance_QRCodeUpdated;
        QRCodesManager.Instance.QRCodeRemoved += Instance_QRCodeRemoved;
        pendingActions = new Queue<ActionData>();
        if (MenuWrapper == null)
        {
            throw new System.Exception("Menu wrapper not assigned");
        }
        if (SceneManager.GetActiveScene().name == "Study-HapticButtonSurvey" && StartCube == null)
        {
            throw new System.Exception("Start cube not assigned");
        }
    }
    private void Instance_QRCodesTrackingStateChanged(object sender, bool status)
    {
        if (!status)
        {
            clearExisting = true;
        }
    }

    private void Instance_QRCodeAdded(object sender, QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode> e)
    {
        Debug.Log("QRCodesVisualizer Instance_QRCodeAdded");

        lock (pendingActions)
        {
            pendingActions.Enqueue(new ActionData(ActionData.Type.Added, e.Data));
        }
    }

    private void Instance_QRCodeUpdated(object sender, QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode> e)
    {
        Debug.Log("QRCodesVisualizer Instance_QRCodeUpdated");

        lock (pendingActions)
        {
            pendingActions.Enqueue(new ActionData(ActionData.Type.Updated, e.Data));
        }
    }

    private void Instance_QRCodeRemoved(object sender, QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode> e)
    {
        Debug.Log("QRCodesVisualizer Instance_QRCodeRemoved");

        lock (pendingActions)
        {
            pendingActions.Enqueue(new ActionData(ActionData.Type.Removed, e.Data));
        }
    }

    private void HandleEvents()
    {
        lock (pendingActions)
        {
            while (pendingActions.Count > 0)
            {
                var action = pendingActions.Dequeue();

                // ==== Beginn Eigenanteil ==== /
                if (action.type == ActionData.Type.Added)
                {
                    if (action.qrCode.Data == "Button9")
                    {
                        this.AddMenuTrackingToQrCode(action.qrCode, false);

                    }
                    else if (action.qrCode.Data == "MenuHover")
                    {
                        this.AddMenuTrackingToQrCode(action.qrCode, true);
                    }
                    else if (action.qrCode.Data == "StartCube" && SceneManager.GetActiveScene().name == "Study-HapticButtonSurvey")
                    {
                        this.AddStartCubeTracking(action.qrCode);
                    }
                    Debug.Log("Enabled tracking on " + action.qrCode.SpatialGraphNodeId);
                }
                else if (action.type == ActionData.Type.Updated)
                {
                    if ((action.qrCode.Data == "Button9" || action.qrCode.Data == "MenuHover") && this.MenuWrapper.GetComponent<SpatialGraphNodeTracker>().Id != action.qrCode.SpatialGraphNodeId)
                    {
                        if (action.qrCode.Data == "Button9")
                        {
                            this.AddMenuTrackingToQrCode(action.qrCode, false);
                        }
                        else if (action.qrCode.Data == "MenuHover")
                        {
                            this.AddMenuTrackingToQrCode(action.qrCode, true);
                        }
                    }
                    else if (action.qrCode.Data == "StartCube" && (SceneManager.GetActiveScene().name == "Study-HapticButtonSurvey" || SceneManager.GetActiveScene().name == "BuildStudy")  && this.StartCube.GetComponent<SpatialGraphNodeTracker>().Id != action.qrCode.SpatialGraphNodeId)
                    {
                        this.AddStartCubeTracking(action.qrCode);
                    }

                    Debug.Log("Enabled tracking on " + action.qrCode.SpatialGraphNodeId);
                } 
            }
        }
    }


    private void AddMenuTrackingToQrCode(QRCode qrCode, bool hoverUi)
    {
        this.MenuWrapper.GetComponent<SpatialGraphNodeTracker>().Id = qrCode.SpatialGraphNodeId;
        this.MenuWrapper.GetComponent<SpatialGraphNodeTracker>().enabled = true;
        this.MenuWrapper.GetComponent<QRCodeVisabilityHandler>().qrCode = qrCode;
        MenuQrCodePlacer[] MenuPlacers = this.MenuWrapper.GetComponentsInChildren<MenuQrCodePlacer>(true);

        foreach (MenuQrCodePlacer MenuPlacer in MenuPlacers)
        {
            MenuPlacer.StopAllCoroutines();
            MenuPlacer.enabled = false;
            MenuPlacer.enabled = true;
            MenuPlacer.qrCode = qrCode;
            MenuPlacer.hover = hoverUi;
        }
    }

    private void AddStartCubeTracking(QRCode qrCode)
    {
        this.StartCube.GetComponent<SpatialGraphNodeTracker>().Id = qrCode.SpatialGraphNodeId;
        this.StartCube.GetComponent<SpatialGraphNodeTracker>().enabled = true;
    }
    // ==== Ende Eigenanteil ==== /

    // Update is called once per frame
    void Update()
    {
        HandleEvents();
    }

}