using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfDevice : MonoBehaviour {

    public bool isAndroid;

    // Use this for initialization
    private void Awake()
    {
        if((isAndroid && SystemInfo.deviceType == DeviceType.Handheld) || (!isAndroid && SystemInfo.deviceType == DeviceType.Desktop))
        {
            Destroy(this.gameObject);
        }
    }
}
