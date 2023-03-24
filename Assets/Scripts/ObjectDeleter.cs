using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class ObjectDeleter : MonoBehaviour, IMixedRealityTouchHandler
{
    /// <summary>
    /// Defines how many frames the delete delay should be
    /// </summary>
    private static int DeleteDelay = 300;
    /// <summary>
    /// Counts the frames since object has been touched
    /// </summary>
    private int DelayCounter = 0;

    private float OpacitySteps = (float)1 / (float)DeleteDelay;

    private bool isTouched = false;
    private Renderer Renderer;
    private Color OriginalColor;

    /// <summary>
    /// Called if the object that this script is attached to is touched
    /// </summary>
    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
        this.Renderer.material.color = Color.red;
        this.isTouched = true;
    }

    /// <summary>
    /// Called if the object that this script is attached to is no longer touched
    /// </summary>
    public void OnTouchCompleted(HandTrackingInputEventData eventData)
    {
        this.isTouched = false;
        this.DelayCounter = 0;
        this.Renderer.material.color = this.OriginalColor;
    }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }

    void OnEnable()
    {
        Renderer = gameObject.GetComponent<Renderer>();
        MaterialUtils.SetupBlendMode(Renderer.material, MaterialUtils.BlendMode.Transparent);
        this.OriginalColor = Renderer.material.color;
    }

    void Update()
    {
        if (isTouched)
        {
            if (DeleteDelay <= this.DelayCounter)
            {
                Destroy(gameObject);
            }
            else
            {
                this.DelayCounter++;
                Color currentColor = Renderer.material.color;
                currentColor.a = (currentColor.a - this.OpacitySteps);
                Renderer.material.color = currentColor;
            }
        }
    }
}
