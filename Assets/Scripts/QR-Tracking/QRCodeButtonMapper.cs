// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.QR;
using Microsoft.MixedReality.SampleQRCodes;
using System.Collections.Generic;
using UnityEngine;


enum MenuState
{
    MainMenu,
    PlaceMenu,
    EditMenu
}

public class QRCodeButtonMapper : MonoBehaviour
{
    public GameObject MenuWrapper;

    private bool isTrackingEnabled = false;
    private bool clearExisting = false;
    private MenuState MenuState = MenuState.MainMenu;

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

    private Queue<ActionData> pendingActions = new Queue<ActionData>();

    // Use this for initialization
    void Start()
    {
        Debug.Log("QRCodesVisualizer start");

        QRCodesManager.Instance.QRCodesTrackingStateChanged += Instance_QRCodesTrackingStateChanged;
        QRCodesManager.Instance.QRCodeAdded += Instance_QRCodeAdded;
        QRCodesManager.Instance.QRCodeUpdated += Instance_QRCodeUpdated;
        QRCodesManager.Instance.QRCodeRemoved += Instance_QRCodeRemoved;
        if (MenuWrapper == null)
        {
            throw new System.Exception("Menu wrapper not assigned");
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


                if (action.type == ActionData.Type.Added && action.qrCode.Data == "Button9")
                {
                    this.AddMenuTrackingToQrCode(action.qrCode);
                    this.isTrackingEnabled = true;
                    Debug.Log("Enabled tracking on " + action.qrCode.SpatialGraphNodeId);

                }
                else if (action.type == ActionData.Type.Updated && action.qrCode.Data == "Button9")
                {
                    if (!isTrackingEnabled)
                    {
                        this.AddMenuTrackingToQrCode(action.qrCode);
                        this.isTrackingEnabled = true;
                        Debug.Log("Enabled tracking on " + action.qrCode.SpatialGraphNodeId);

                    }
                }
                else if (action.type == ActionData.Type.Removed)
                {
                    //Nothing yet
                }
            }
        }
    }

    private void AddMenuTrackingToQrCode(QRCode qrCode)
    {
        this.MenuWrapper.GetComponent<SpatialGraphNodeTracker>().Id = qrCode.SpatialGraphNodeId;
        this.MenuWrapper.GetComponent<SpatialGraphNodeTracker>().enabled = true;
        foreach(MenuQrCodePlacer MenuPlacer in this.MenuWrapper.GetComponentsInChildren<MenuQrCodePlacer>(true))
        {
            MenuPlacer.qrCode = qrCode;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleEvents();
    }

}