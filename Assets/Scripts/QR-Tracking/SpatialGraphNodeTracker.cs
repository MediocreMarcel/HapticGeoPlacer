// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.OpenXR;

namespace Microsoft.MixedReality.SampleQRCodes
{
    internal class SpatialGraphNodeTracker : MonoBehaviour
    {
        private SpatialGraphNode node;

        public System.Guid Id { get; set; }

        void Update()
        {
            if (node == null || node.Id != Id)
            {
                node = (Id != System.Guid.Empty) ? SpatialGraphNode.FromStaticNodeId(Id) : null;
                //Debug.Log("Initialize SpatialGraphNode Id= " + Id);
            }

            if (node != null)
            {
                if (node.TryLocate(FrameTime.OnUpdate ,out Pose pose))
                {
                    // If there is a parent to the camera that means we are using teleport and we should not apply the teleport
                    // to these objects so apply the inverse
                    if (CameraCache.Main.transform.parent != null)
                    {
                        pose = pose.GetTransformedBy(CameraCache.Main.transform.parent);
                    }
                    //Adapt Rotation
                    Quaternion rotation = pose.rotation;
                    Vector3 rotationEulerAngles = rotation.eulerAngles;
                    rotationEulerAngles.x = rotationEulerAngles.x + 180;
                    rotationEulerAngles.z = -rotationEulerAngles.z;
                    rotation.eulerAngles = rotationEulerAngles;
                    
                    gameObject.transform.SetPositionAndRotation(pose.position, rotation);
                   // Debug.Log("Id= " + Id + " QRPose = " + pose.position.ToString("F7") + " QRRot = " + pose.rotation.ToString("F7"));
                }
                else
                {
                    Debug.LogWarning("Cannot locate " + Id);
                }
            } else
            {
                //Debug.Log("Node is null");
            }
        }
    }
}