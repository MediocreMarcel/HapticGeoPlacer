using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderPlacer : Placeable
{
    public GameObject CylinderPrefab;

    public override void GeneratePreview(Vector3 centerPosition, Vector3 scale)
    {
        if (this.previewObject == null)
        {
            this.previewObject = Instantiate(CylinderPrefab, centerPosition, Quaternion.identity).gameObject;
        }

        this.SetPreviewMaterial(this.previewObject.GetComponent<Renderer>().material);

        scale = new Vector3(scale.x, scale.y / 2, scale.z);

        this.previewObject.transform.position = centerPosition;
        this.previewObject.transform.localScale = scale;
    }

}
