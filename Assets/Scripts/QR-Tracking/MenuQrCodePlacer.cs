using Microsoft.MixedReality.QR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuQrCodePlacer : MonoBehaviour
{
    public QRCode qrCode { get; set; }
    // ==== Beginn Eigenanteil ==== /
    public float QRMargin = 0.012f;
    public bool hover;

    [SerializeField] private GameObject BackplateQuad;
    private Renderer BackplateQuadRenderer;
    private long lastUpdate;
    private Coroutine LerpCoroutine;
    private LerpUtil LerpUtil = new LerpUtil();
    private Vector3 PreviousFrameMenuPosition = new Vector3(0, 0, 0);


    private void Start()
    {
        this.BackplateQuadRenderer = BackplateQuad.GetComponent<Renderer>();
    }

    void Update()
    {
        // ==== Ende Eigenanteil ==== /
        if (qrCode != null && lastUpdate != qrCode.SystemRelativeLastDetectedTime.Ticks)
        {
            // ==== Beginn Eigenanteil ==== /
            if (this.LerpCoroutine != null)
            {
                StopCoroutine(this.LerpCoroutine);
            }
            float originalQRCodeSize = qrCode.PhysicalSideLength;
            Vector3 backplateSize = this.BackplateQuadRenderer.bounds.size;
            Vector3 desiredMenuPosition;
            if (hover)
            {
                desiredMenuPosition = new Vector3(originalQRCodeSize + (backplateSize.x / 2.0f) + 0.005f + QRMargin, -(backplateSize.y / 2.0f) + QRMargin, 0.0f);
            }
            else
            {
                desiredMenuPosition = new Vector3(originalQRCodeSize - (backplateSize.x / 2.0f) - 0.005f + QRMargin, -(backplateSize.y / 2.0f) + QRMargin, 0.0f);
            }

            this.lastUpdate = qrCode.SystemRelativeLastDetectedTime.Ticks;
           /* if (Vector3.Distance(this.PreviousFrameMenuPosition, desiredMenuPosition) > 0.001f)
            {*/
                this.LerpCoroutine = StartCoroutine(this.LerpUtil.LerpLocalPosition(gameObject, desiredMenuPosition, 0.3f));
                Debug.Log("Move X: " + desiredMenuPosition.x);
           /* }
            this.PreviousFrameMenuPosition = desiredMenuPosition;*/
            // ==== Ende Eigenanteil ==== /
        }
    }
}
