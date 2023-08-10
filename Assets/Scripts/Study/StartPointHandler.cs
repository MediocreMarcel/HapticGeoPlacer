using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointHandler : MonoBehaviour, IMixedRealityTouchHandler
{
    public int OnTouchLockFrames = 60;
    private int OnTouchLockFramesCounter = 0;
    [SerializeField] private ButtonStudyHandler ButtonStudyHandler;
    public void OnTouchCompleted(HandTrackingInputEventData eventData)
    {
        ButtonStudyHandler.OnStartCubeReleased();
    }

    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
        if (this.OnTouchLockFramesCounter <= 0)
        {
            this.OnTouchLockFramesCounter = this.OnTouchLockFrames;
            ButtonStudyHandler.onStartCubeTouched();          
        } else
        {

        }
        
    }

    public void OnTouchUpdated(HandTrackingInputEventData eventData)
    {
        //Do nothing
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
