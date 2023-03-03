using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuboidPlacer : Placeable
{
    public override void GeneratePreview(MixedRealityPose pose)
    {
        if (this.previewObject == null)
        {
            this.previewObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        Material material = this.previewObject.GetComponent<Renderer>().material;

        MaterialUtils.SetupBlendMode(material, MaterialUtils.BlendMode.Transparent);
        material.color = new Color(0.52f, 0.52f, 0.52f, 0.7f);

        Vector3 centerPos = (this.startPosition + pose.Position) / 2;

        float scaleX = Mathf.Abs(this.startPosition.x - pose.Position.x);
        float scaleY = Mathf.Abs(this.startPosition.y - pose.Position.y);
        float scaleZ = Mathf.Abs(this.startPosition.z - pose.Position.z);

        this.previewObject.transform.position = centerPos;
        this.previewObject.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

}
