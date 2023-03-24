using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class MainMenuHandler : MonoBehaviour
{

    [SerializeField] private Interactable DeleteToggleButton;

    /// <summary>
    /// Handels Toggle Delete Button Action. Enables all delete scripts on placed objects.
    /// </summary>
    /// <param name="isToggled">State of delete toggle button</param>
    public void OnDeleteToggle(bool isToggled)
    {
        GameObject[] Figures = GameObject.FindGameObjectsWithTag("PlacedFigure");
        foreach (GameObject Figure in Figures)
        {
            ObjectDeleter objectDeleter = Figure.GetComponent<ObjectDeleter>();
            objectDeleter.enabled = isToggled;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Gently closes the Menu by deactivating all toggle button functions
    /// </summary>
    public void CloseMenu()
    {
        this.DeleteToggleButton.IsToggled = false;
        this.OnDeleteToggle(false);
        gameObject.SetActive(false);
    }
}
