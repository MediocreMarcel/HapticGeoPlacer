using Microsoft.MixedReality.QR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuQrCodePlacer : MonoBehaviour
{
    public QRCode qrCode { get; set; }
    public float QRMargin = 0.012f;

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
        if (qrCode != null && lastUpdate != qrCode.SystemRelativeLastDetectedTime.Ticks)
        {
            if (this.LerpCoroutine != null)
            {
                StopCoroutine(this.LerpCoroutine);
            }
            float originalQRCodeSize = qrCode.PhysicalSideLength;
            Vector3 backplateSize = this.BackplateQuadRenderer.bounds.size;
            Vector3 desiredMenuPosition = new Vector3(originalQRCodeSize - (backplateSize.x / 2.0f) - 0.005f + QRMargin, -(backplateSize.y / 2.0f) + QRMargin, 0.0f);
            this.lastUpdate = qrCode.SystemRelativeLastDetectedTime.Ticks;
            if (Vector3.Distance(this.PreviousFrameMenuPosition, desiredMenuPosition) > 0.005f)
            {
                this.LerpCoroutine = StartCoroutine(this.LerpUtil.LerpLocalPosition(gameObject, desiredMenuPosition, 0.3f));
            }
            this.PreviousFrameMenuPosition = desiredMenuPosition;
        }
    }
}
