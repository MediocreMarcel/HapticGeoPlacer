using Microsoft.MixedReality.SampleQRCodes;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointHandler : MonoBehaviour, IMixedRealityTouchHandler
{
    public int OnTouchLockFrames = 60;
    private int OnTouchLockFramesCounter = 0;
    private SpatialGraphNodeTracker spatialGraphNodeTracker;
    [SerializeField] private ButtonStudyHandler ButtonStudyHandler;
    public void OnTouchCompleted(HandTrackingInputEventData eventData)
    {
        Debug.Log("Start Cube Released");
        this.spatialGraphNodeTracker.pauseTracking = false;
        ButtonStudyHandler.OnStartCubeReleased();
    }

    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
        if (this.OnTouchLockFramesCounter <= 0)
        {
            Debug.Log("Start Cube Started");
            this.OnTouchLockFramesCounter = this.OnTouchLockFrames;
            this.spatialGraphNodeTracker.pauseTracking = true;
            ButtonStudyHandler.onStartCubeTouched();          
        } else
        {
            Debug.Log("Start Cube Locked");
        }
        
    }

    public void OnTouchUpdated(HandTrackingInputEventData eventData)
    {
        //Do nothing
    }

    // Start is called before the first frame update
    void Start()
    {
        this.spatialGraphNodeTracker = this.GetComponentInParent<SpatialGraphNodeTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if(OnTouchLockFramesCounter > 0)
        {
            OnTouchLockFramesCounter--;
        }
    }
}
