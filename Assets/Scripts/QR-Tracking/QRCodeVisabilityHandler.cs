using Microsoft.MixedReality.QR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QRCodeVisabilityHandler : MonoBehaviour
{
    public UnityEvent OnQRCodeNotVisible;
    public UnityEvent OnQRCodeVisible;
    public QRCode qrCode;
    public double InvisibilityTriggerTime = 2.0;
    private bool invisible = false;


    // Update is called once per frame
    void Update()
    {
        if (qrCode != null)
        {
            double qrCodeLastSeen = System.DateTime.Now.Subtract(qrCode.LastDetectedTime.DateTime).TotalSeconds;
            if (!this.invisible && qrCodeLastSeen >= InvisibilityTriggerTime)
            {
                this.invisible = true;
                this.OnQRCodeNotVisible.Invoke();
            }
            else if (this.invisible && qrCodeLastSeen < InvisibilityTriggerTime)
            {
                this.invisible = false;
                this.OnQRCodeVisible.Invoke();
            }
        }

    }
}
