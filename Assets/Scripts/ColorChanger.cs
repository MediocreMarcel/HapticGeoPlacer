using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class ColorChanger : MonoBehaviour, IMixedRealityTouchHandler
{
    [SerializeField] public ColorMenuHandler ColorMenuHandler;
    [SerializeField] public ButtonStudyHandler ButtonStudyHandler;

    /// <summary>
    /// Called if Object that this script is attached to is touched.
    /// </summary>
    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
        Color? color = this.ColorMenuHandler.GetSelectedColorAsUnityColor();
        if (color != null)
        {
            gameObject.GetComponent<Renderer>().material.color = (Color)color;
            this.invokeColorChangeOnStudy((FigurColors) this.ColorMenuHandler.GetSelectedColor());

        }
    }
    public void OnTouchCompleted(HandTrackingInputEventData eventData) { }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }

    private void Update() { }

    private void invokeColorChangeOnStudy(FigurColors color)
    {
        if (this.ButtonStudyHandler != null)
        {
            this.ButtonStudyHandler.OnColorDone((int)color);
        }
    }

}
