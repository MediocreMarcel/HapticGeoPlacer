using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;


public class Placer : MonoBehaviour, IMixedRealityPointerHandler
{
    private State state = State.Idle;
    private Vector3 startPosition;
    [SerializeField] private GameObject startMarker;
    [SerializeField] private GameObject endMarker;
    [SerializeField] private GameObject startSphere;
    [SerializeField] private GameObject endSphere;
    private GameObject cubePreview;

    private float sphereSize = 0.03f;

    private void OnEnable()
    {
        // Instruct Input System that we would like to receive all input events of type IMixedRealityGestureHandler
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
    }

    private void OnDisable()
    {
        // Instruct Input System to disregard all input events of type IMixedRealityGestureHandler
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityPointerHandler>(this);
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        if (state == State.Idle)
        {
            this.startPosition = eventData.Pointer.Position;
            this.state = State.StartPlaced;
            if(this.startMarker == null)
            {
                this.startMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            }
            this.startMarker.SetActive(true);
            this.startSphere.transform.localScale = new Vector3(this.sphereSize, this.sphereSize, this.sphereSize);
            this.startSphere.GetComponent<Renderer>().material.color = new Color(0, 255, 0);

            this.startMarker.transform.localPosition = this.startPosition;
        }
        else if (state == State.StartPlaced && this.cubePreview != null)
        {
            this.state = State.Idle;
            this.cubePreview.GetComponent<Renderer>().material.color = new Color(0.066f, 0.122f, 0.412f);
            this.startMarker.SetActive(false);
            this.endMarker.SetActive(false);
            this.startPosition = Vector3.zero;
            this.cubePreview = null;
        }
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        //Debug.Log($"Event: {eventData.ToString()}, Position: {eventData.Pointer.Position}");
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        // Debug.Log($"Event: {eventData.ToString()}, Position: {eventData.Pointer}");
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        //Debug.Log($"Event: {eventData.ToString()}, Position: {eventData.Pointer}");
    }

    // Update is called once per frame
    void Update()
    {
        MixedRealityPose pose;
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Both, out pose))
        {
            if (state == State.Idle)
            {
                this.updateStartSphere(pose);

            }
            else if (state == State.StartPlaced)
            {
                this.updateCubePreview(pose);
                this.updateEndSphere(pose);
            }

        }

    }

    private void updateCubePreview(MixedRealityPose pose)
    {
        if (cubePreview == null)
        {
            this.cubePreview = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        Material material = this.cubePreview.GetComponent<Renderer>().material;

        MaterialUtils.SetupBlendMode(material, MaterialUtils.BlendMode.Transparent);
        material.color = new Color(0.52f, 0.52f, 0.52f, 0.7f);

        Vector3 centerPos = (this.startPosition + pose.Position) / 2;

        float scaleX = Mathf.Abs(this.startPosition.x - pose.Position.x);
        float scaleY = Mathf.Abs(this.startPosition.y - pose.Position.y);
        float scaleZ = Mathf.Abs(this.startPosition.z - pose.Position.z);

        this.cubePreview.transform.position = centerPos;
        this.cubePreview.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    private void updateStartSphere(MixedRealityPose pose)
    {
        if (this.startSphere == null)
        {
            this.startSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }
        if (this.startMarker.activeSelf == false)
        {
            this.startMarker.SetActive(true);
        }

        this.startSphere.transform.localScale = new Vector3(this.sphereSize, this.sphereSize, this.sphereSize);
        this.startSphere.transform.localPosition = pose.Position;
        this.startSphere.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
    }

    private void updateEndSphere(MixedRealityPose pose)
    {
        if (this.endSphere == null)
        {
            this.endSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }
        if (this.endMarker.activeSelf == false)
        {
            this.endMarker.SetActive(true);
        }

        this.endSphere.transform.localScale = new Vector3(this.sphereSize, this.sphereSize, this.sphereSize);
        this.endSphere.GetComponent<Renderer>().material.color = new Color(255, 0, 0);

        this.endMarker.transform.localPosition = pose.Position;
    }
}
