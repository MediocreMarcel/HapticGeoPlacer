// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.QR;
using Microsoft.MixedReality.SampleQRCodes;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


enum MenuState
{
    MainMenu,
    PlaceMenu,
    EditMenu
}

public class QRCodeButtonMapper : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject PlaceMenu;

    private SortedDictionary<System.Guid, GameObject> qrCodesObjectsList;
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
        qrCodesObjectsList = new SortedDictionary<System.Guid, GameObject>();

        QRCodesManager.Instance.QRCodesTrackingStateChanged += Instance_QRCodesTrackingStateChanged;
        QRCodesManager.Instance.QRCodeAdded += Instance_QRCodeAdded;
        QRCodesManager.Instance.QRCodeUpdated += Instance_QRCodeUpdated;
        QRCodesManager.Instance.QRCodeRemoved += Instance_QRCodeRemoved;
        if (MainMenu == null)
        {
            throw new System.Exception("Main Menu not assigned");
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


                if (action.type == ActionData.Type.Added)
                {
                    try
                    {
                        this.qrCodesObjectsList.Add(action.qrCode.Id, this.AddButtonTrackingToQrCode(action.qrCode));
                        Debug.Log("Enabled tracking on " + action.qrCode.SpatialGraphNodeId);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        // Do nothing
                        Debug.LogWarning("QR Button number out of range: " + action.qrCode.Data);
                    }
                    catch (FormatException e)
                    {
                        Debug.LogWarning("Found QR Code in wrong format" + action.qrCode.Data);
                    }

                }
                else if (action.type == ActionData.Type.Updated && action.qrCode.Data == "Button1")
                {
                    if (!qrCodesObjectsList.ContainsKey(action.qrCode.Id))
                    {
                        try
                        {
                            this.qrCodesObjectsList.Add(action.qrCode.Id, this.AddButtonTrackingToQrCode(action.qrCode));
                            Debug.Log("Enabled tracking on " + action.qrCode.SpatialGraphNodeId);
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            // Do nothing
                            Debug.LogWarning("QR Button number out of range: " + action.qrCode.Data);
                        }
                        catch (FormatException e)
                        {
                            Debug.LogWarning("Found QR Code in wrong format" + action.qrCode.Data);
                        }
                    }
                }
                else if (action.type == ActionData.Type.Removed)
                {
                    if (qrCodesObjectsList.ContainsKey(action.qrCode.Id))
                    {
                        Debug.Log("Removed");
                    }
                }
            }
        }
        if (clearExisting)
        {
            clearExisting = false;
            foreach (var obj in qrCodesObjectsList)
            {
                Destroy(obj.Value);
            }
            qrCodesObjectsList.Clear();

        }
    }

    private GameObject AddButtonTrackingToQrCode(QRCode qrCode)
    {
        Match buttonStringRegexMatches = Regex.Match(qrCode.Data, "Button\\d$");
        if (buttonStringRegexMatches.Success)
        {
            int buttonNumber = Int32.Parse(buttonStringRegexMatches.Value[6].ToString()) - 1;
            switch (this.MenuState)
            {
                case MenuState.MainMenu:
                    Debug.Log(buttonStringRegexMatches.Value[6]);
                    Debug.Log(buttonNumber);
                    if (buttonNumber >= 0 && buttonNumber < 3)
                    {
                        GameObject ButtonWrapper = this.MainMenu.transform.GetChild(buttonNumber).gameObject;
                        ButtonWrapper.GetComponent<SpatialGraphNodeTracker>().Id = qrCode.SpatialGraphNodeId;
                        ButtonWrapper.GetComponent<SpatialGraphNodeTracker>().enabled = true;
                        GameObject Button = ButtonWrapper.transform.GetChild(0).gameObject;
                        Button.GetComponent<QRSizeMapper>().qrCode = qrCode;
                        Button.GetComponent<QRSizeMapper>().enabled = true;
                        return ButtonWrapper;
                    }
                    throw new ArgumentOutOfRangeException("Button Data id is out of range for the current menu type.");
            }
        }
        throw new FormatException("Button Data does not match expected format");
    }

    // Update is called once per frame
    void Update()
    {
        HandleEvents();
    }
}