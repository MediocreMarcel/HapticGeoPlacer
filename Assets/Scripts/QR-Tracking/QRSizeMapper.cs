using Microsoft.MixedReality.QR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QRSizeMapper : MonoBehaviour
{

    public QRCode qrCode { get; set; }

    private long lastUpdate;

    void Update()
    {
        if(qrCode != null && lastUpdate != qrCode.SystemRelativeLastDetectedTime.Ticks)
        {
            float originalQRCodeSize = qrCode.PhysicalSideLength;
            gameObject.transform.localPosition = new Vector3(originalQRCodeSize / 2.0f, -originalQRCodeSize / 2.0f, -0.01f);
            this.lastUpdate = qrCode.SystemRelativeLastDetectedTime.Ticks;
        }
        
    }
}
