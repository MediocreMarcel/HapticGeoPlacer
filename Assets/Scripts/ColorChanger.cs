using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class ColorChanger : MonoBehaviour, IMixedRealityTouchHandler
{
    [SerializeField] public ColorMenuHandler ColorMenuHandler;

    /// <summary>
    /// Called if Object that this script is attached to is touched.
    /// </summary>
    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
        Color? color = this.ColorMenuHandler.GetSelectedColor();
        if (color != null)
        {
            gameObject.GetComponent<Renderer>().material.color = (Color)color;
        }
    }
    public void OnTouchCompleted(HandTrackingInputEventData eventData) { }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }

    private void Update() { }

}
