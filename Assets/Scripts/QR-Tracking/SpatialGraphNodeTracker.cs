// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.OpenXR;
using UnityEngine.Events;
using System.Collections;

namespace Microsoft.MixedReality.SampleQRCodes
{
    internal class SpatialGraphNodeTracker : MonoBehaviour
    {
        private SpatialGraphNode node;

        public System.Guid Id { get; set; }
        public UnityEvent OnQRCodeNotVisible;
        public UnityEvent OnQRCodeVisible;

        private bool NodeVisible = false;
        private Coroutine PositionLerpCoroutine;
        private Coroutine RotationLerpCoroutine;
        private LerpUtil LerpUtil = new LerpUtil();
        private Vector3 PreviousFramePosition = new Vector3(0, 0, 0);
        private Quaternion PreviousFrameRotation = new Quaternion(0, 0, 0, 0);


        void Update()
        {
            if (node == null || node.Id != Id)
            {
                node = (Id != System.Guid.Empty) ? SpatialGraphNode.FromStaticNodeId(Id) : null;
                //Debug.Log("Initialize SpatialGraphNode Id= " + Id);
            }

            if (node != null)
            {
                if (node.TryLocate(FrameTime.OnUpdate, out Pose pose))
                {
                    if (!this.NodeVisible)
                    {
                        this.NodeVisible = true;
                        this.OnQRCodeVisible.Invoke();
                    }

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

                    if (Vector3.Distance(this.PreviousFramePosition, pose.position) > 0.005f)
                    {
                        if (this.PositionLerpCoroutine != null)
                        {
                            StopCoroutine(this.PositionLerpCoroutine);
                        }
                        this.PositionLerpCoroutine = StartCoroutine(this.LerpUtil.LerpPosition(gameObject, pose.position, 0.3f));
                    }

                    if (Quaternion.Angle(this.PreviousFrameRotation, pose.rotation) > 0.005f)
                    {
                        if (this.RotationLerpCoroutine != null)
                        {
                            StopCoroutine(this.RotationLerpCoroutine);
                        }
                        this.RotationLerpCoroutine = StartCoroutine(this.LerpUtil.LerpRotation(gameObject, rotation, 0.3f));
                    }

                    this.PreviousFramePosition = pose.position;
                    this.PreviousFrameRotation = pose.rotation;

                    //Debug.Log("Id= " + Id + " QRPose = " + pose.position.ToString("F7") + " QRRot = " + pose.rotation.ToString("F7"));
                }
                else
                {
                    if (this.NodeVisible)
                    {
                        this.NodeVisible = false;
                        this.OnQRCodeNotVisible.Invoke();
                    }
                    PreviousFramePosition = new Vector3(0, 0, 0);
                    PreviousFrameRotation = new Quaternion(0, 0, 0, 0);
                }
            }
        }
    }
}