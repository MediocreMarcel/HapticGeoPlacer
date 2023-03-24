using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
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
    public GameObject previewOutlinePrefab;

    protected Vector3 startPosition;
    protected GameObject previewObject;

    private GameObject previewOutline;

    private const float SPHERE_SIZE = 0.03f;

    private State state = State.Idle;
    private bool isPaused = false;

    [SerializeField] private GameObject startMarker;
    [SerializeField] private GameObject endMarker;
    [SerializeField] private GameObject startSphere;
    [SerializeField] private GameObject endSphere;

    /// <summary>
    /// Method generates the figure as a preview.
    /// </summary>
    /// <param name="centerPosition">Center position of the figure</param>
    /// <param name="scale">Scale of the figure</param>
    public abstract void GeneratePreview(Vector3 centerPosition, Vector3 scale);

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
            this.endMarker.SetActive(false);
        }
        if (this.previewOutline == null)
        {
            this.previewOutline = Instantiate(this.previewOutlinePrefab, Vector3.zero, Quaternion.identity).gameObject;
            this.previewOutline.SetActive(false);
        }

        this.startMarker.SetActive(true);
        this.endMarker.SetActive(false);
    }

    private void OnDisable()
    {
        // Instruct Input System to disregard all input events of type IMixedRealityGestureHandler
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityPointerHandler>(this);

        this.startPosition = Vector3.zero;
        this.previewObject = null;
        this.previewOutline = null;
        this.state = State.Idle;
        this.startMarker.SetActive(false);
        this.endMarker.SetActive(false);
    }

    /// <summary>
    /// Pauses the placer. Keeps the Start point if its allready been set.
    /// </summary>
    /// <param name="paused">Bool true if paused</param>
    public void SetPausePlacer(bool paused)
    {
        this.isPaused = paused;
        this.startMarker.SetActive(!paused);
        this.endMarker.SetActive(!paused);
        this.previewOutline.SetActive(!paused && this.state == State.StartPlaced);
        if (paused)
        {
            Destroy(this.previewObject);
        }
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData) { }

    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }

    public void OnPointerUp(MixedRealityPointerEventData eventData) { }

    /// <summary>
    /// Called if the pinch gesture is recognized
    /// </summary>
    /// <param name="eventData">event data given from MRTK</param>
    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        if (!this.isPaused)
        {
            if (state == State.Idle)
            {
                this.startPosition = this.startMarker.transform.position;
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
                this.previewOutline.SetActive(false);
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
                this.UpdateStartSphere(pose);
            }
            else if (state == State.StartPlaced)
            {
                Vector3 previewCenterPosition = this.CalucaltePrewiewCenterPosition(pose);
                Vector3 previewScale = this.CalculatePreviewScale(pose);
                this.GeneratePreview(previewCenterPosition, previewScale);
                this.DrawPreviewOutline(previewCenterPosition, previewScale);
                this.UpdateEndSphere(pose);
            }
        }
        else
        {
            this.startMarker.SetActive(false);
            this.endMarker.SetActive(false);
            this.previewOutline.SetActive(false);
        }
    }

    /// <summary>
    /// Changes a material to the preview material
    /// </summary>
    /// <param name="material">Material that should be modified</param>
    protected void SetPreviewMaterial(Material material)
    {
        MaterialUtils.SetupBlendMode(material, MaterialUtils.BlendMode.Transparent);
        material.color = new Color(0.52f, 0.52f, 0.52f, 0.7f);
    }

    /// <summary>
    /// Draws a Box around the preview object
    /// </summary>
    /// <param name="position">Position of the Box</param>
    /// <param name="scale">Scale of the Box</param>
    private void DrawPreviewOutline(Vector3 position, Vector3 scale)
    {
        if (this.previewOutline == null)
        {
            this.previewOutline = Instantiate(this.previewOutlinePrefab, position, Quaternion.identity).gameObject;
        }
        if (!this.previewOutline.activeSelf)
        {
            this.previewOutline.SetActive(true);
        }

        this.previewOutline.transform.position = position;
        this.previewOutline.transform.localScale = scale;

    }

    /// <summary>
    /// Calculates the Scale that a figure needs
    /// </summary>
    /// <param name="pose">Current position of index finger as MixRealityPose Object</param>
    /// <returns>Scale that the object needs</returns>
    private Vector3 CalculatePreviewScale(MixedRealityPose pose)
    {
        float scaleX = Mathf.Abs(this.startPosition.x - pose.Position.x);
        float scaleY = Mathf.Abs(this.startPosition.y - pose.Position.y);
        float scaleZ = Mathf.Abs(this.startPosition.z - pose.Position.z);
        return new Vector3(scaleX, scaleY, scaleZ);
    }

    /// <summary>
    /// Calculates the center position of an object, so it can be placed between two given points.
    /// </summary>
    /// <param name="pose">Current position of index finger as MixRealityPose Object</param>
    /// <returns>Center position of object between start point and current finger position</returns>
    private Vector3 CalucaltePrewiewCenterPosition(MixedRealityPose pose)
    {
        return (this.startPosition + pose.Position) / 2;
    }


    /// <summary>
    /// Updates the Position of the start sphere to the current position of the finger
    /// </summary>
    /// <param name="pose">Current position of index finger as MixRealityPose Object</param>
    private void UpdateStartSphere(MixedRealityPose pose)
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

    /// <summary>
    /// Updates the Position of the end sphere to the current position of the finger
    /// </summary>
    /// <param name="pose">Current position of index finger as MixRealityPose Object</param>
    private void UpdateEndSphere(MixedRealityPose pose)
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
