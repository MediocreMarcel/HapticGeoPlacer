using Microsoft.MixedReality.QR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuQrCodePlacer : MonoBehaviour
{
    public QRCode qrCode { get; set; }

    [SerializeField] private GameObject BackplateQuad;
    private Renderer BackplateQuadRenderer;
    private long lastUpdate;

    private void Start()
    {
        this.BackplateQuadRenderer = BackplateQuad.GetComponent<Renderer>();
    }

    void Update()
    {
        if (qrCode != null && lastUpdate != qrCode.SystemRelativeLastDetectedTime.Ticks)
        {
            float originalQRCodeSize = qrCode.PhysicalSideLength;
            Vector3 backplateSize = this.BackplateQuadRenderer.bounds.size;
            Vector3 NewPosition = new Vector3(originalQRCodeSize - (backplateSize.x / 2), -(backplateSize.y / 2), 0.0f);
            gameObject.transform.localPosition = NewPosition;
            this.lastUpdate = qrCode.SystemRelativeLastDetectedTime.Ticks;
        }

    }
}
