using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum State
{
    Idle,
    StartPlaced,
    Paused
}

public abstract class Placeable : MonoBehaviour, IMixedRealityPointerHandler
{
    public UnityEvent onFinishedPlacing;

    protected Vector3 startPosition;
    protected GameObject previewObject;

    private const float SPHERE_SIZE = 0.03f;

    private State state = State.Idle;
    private bool isPaused = false;

    [SerializeField] private GameObject startMarker;
    [SerializeField] private GameObject endMarker;
    [SerializeField] private GameObject startSphere;
    [SerializeField] private GameObject endSphere;

    public abstract void GeneratePreview(MixedRealityPose pose);

    private void OnEnable()
    {
        // Instruct Input System that we would like to receive all input events of type IMixedRealityGestureHandler
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);

        //create simple sphere if markers or sphere objects are null
        if (this.startMarker == null || this.startSphere == null)
        {
            this.startMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            this.startSphere = this.startMarker;
            this.startSphere.transform.localScale = new Vector3(Placeable.SPHERE_SIZE, Placeable.SPHERE_SIZE, Placeable.SPHERE_SIZE);
            this.startSphere.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
        if (this.endMarker == null || this.endSphere == null)
        {
            this.endMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            this.endSphere = this.endMarker;
            this.endSphere.transform.localScale = new Vector3(Placeable.SPHERE_SIZE, Placeable.SPHERE_SIZE, Placeable.SPHERE_SIZE);
            this.endSphere.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }

        this.startMarker.SetActive(true);
    }

    private void OnDisable()
    {
        // Instruct Input System to disregard all input events of type IMixedRealityGestureHandler
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityPointerHandler>(this);

        this.startPosition = Vector3.zero;
        Destroy(this.previewObject);
        this.previewObject = null;
        this.state = State.Idle;
        this.startMarker.SetActive(false);
        this.endMarker.SetActive(false);
    }

    public void SetPausePlacer(bool paused)
    {
        this.isPaused = paused;
        this.startMarker.SetActive(!paused);
        this.endMarker.SetActive(!paused);
        if (paused)
        {
            Destroy(this.previewObject);
        }
        
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        //Debug.Log($"Event: {eventData.ToString()}, Position: {eventData.Pointer.Position}");
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        //Debug.Log($"Event: {eventData.ToString()}, Position: {eventData.Pointer.Position}");
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        //Debug.Log($"Event: {eventData.ToString()}, Position: {eventData.Pointer.Position}");
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        if (!this.isPaused)
        {
            if (state == State.Idle)
            {
                this.startPosition = eventData.Pointer.Position;
                this.state = State.StartPlaced;
            }
            else if (state == State.StartPlaced && this.previewObject != null)
            {
                this.state = State.Idle;
                this.previewObject.GetComponent<Renderer>().material.color = new Color(0.066f, 0.122f, 0.412f);
                this.startMarker.SetActive(false);
                this.endMarker.SetActive(false);
                this.startPosition = Vector3.zero;
                this.previewObject = null;
                this.onFinishedPlacing.Invoke();
            }
        }

    }

    void Update()
    {
        MixedRealityPose pose;
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Both, out pose) && !this.isPaused)
        {
            if (state == State.Idle)
            {
                this.updateStartSphere(pose);
            }
            else if (state == State.StartPlaced)
            {
                this.GeneratePreview(pose);
                this.updateEndSphere(pose);
            }
        }
        else
        {
            this.startMarker.SetActive(false);
            this.endMarker.SetActive(false);
        }
    }

    private void updateStartSphere(MixedRealityPose pose)
    {
        if (this.startSphere == null)
        {
            this.startSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            this.startSphere.transform.localScale = new Vector3(Placeable.SPHERE_SIZE, Placeable.SPHERE_SIZE, Placeable.SPHERE_SIZE);
            this.startSphere.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }

        if (this.startMarker.activeSelf == false)
        {
            this.startMarker.SetActive(true);
        }

        this.startMarker.transform.localPosition = pose.Position;
    }

    private void updateEndSphere(MixedRealityPose pose)
    {
        if (this.endSphere == null)
        {
            this.endSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            this.endSphere.transform.localScale = new Vector3(Placeable.SPHERE_SIZE, Placeable.SPHERE_SIZE, Placeable.SPHERE_SIZE);
            this.endSphere.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        }

        if (this.endMarker.activeSelf == false)
        {
            this.endMarker.SetActive(true);
        }

        this.endMarker.transform.localPosition = pose.Position;
    }

}
