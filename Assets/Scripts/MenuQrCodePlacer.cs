using Microsoft.MixedReality.QR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuQrCodePlacer : MonoBehaviour
{
    public QRCode qrCode { get; set; }
    public float LerpDurationFrames = 60.0f;

    [SerializeField] private GameObject BackplateQuad;
    private Renderer BackplateQuadRenderer;
    private long lastUpdate;
    private Vector3 DesiredMenuPosition;

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
            this.DesiredMenuPosition = new Vector3(originalQRCodeSize - (backplateSize.x / 2.0f) - 0.005f, -(backplateSize.y / 2.0f), 0.0f);
            this.lastUpdate = qrCode.SystemRelativeLastDetectedTime.Ticks;
        }
        if (Vector3.Distance(gameObject.transform.localPosition, DesiredMenuPosition) > 0.001f)
        {
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, this.DesiredMenuPosition, 0.5f * Time.deltaTime);
        }
        else
        {
            gameObject.transform.localPosition = this.DesiredMenuPosition;
        }

    }
}
