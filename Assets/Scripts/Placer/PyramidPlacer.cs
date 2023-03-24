using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidPlacer : Placeable
{
    public GameObject PyramidPrefab;
    public override void GeneratePreview(Vector3 centerPosition, Vector3 scale)
    {
        if (this.previewObject == null)
        {
            this.previewObject = Instantiate(PyramidPrefab, centerPosition, Quaternion.identity).gameObject;
        }

        this.SetPreviewMaterial(this.previewObject.GetComponent<Renderer>().material);

        this.previewObject.transform.position = centerPosition;
        this.previewObject.transform.localScale = scale;
    }

}
