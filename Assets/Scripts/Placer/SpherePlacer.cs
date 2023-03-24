using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class SpherePlacer : Placeable
{
    public GameObject SpherePrefab;
    public override void GeneratePreview(Vector3 centerPosition, Vector3 scale)
    {
        if (this.previewObject == null)
        {
            this.previewObject = Instantiate(SpherePrefab, centerPosition, Quaternion.identity).gameObject;
        }

        this.SetPreviewMaterial(this.previewObject.GetComponent<Renderer>().material);

        this.previewObject.transform.position = centerPosition;
        this.previewObject.transform.localScale = scale;
    }

}