using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPlaceSubmenuIfAllreadyToggled : MonoBehaviour
{

    [SerializeField] private GameObject placeTooggleBar;
    [SerializeField] private Interactable placeButtonInteractable;

    void OnEnable()
    {
        if (placeButtonInteractable.IsToggled)
        {
            placeTooggleBar.SetActive(true);
        }
        
    }

}
