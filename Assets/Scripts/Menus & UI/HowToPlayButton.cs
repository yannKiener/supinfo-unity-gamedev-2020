using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayButton : MonoBehaviour
{
    public GameObject menuToClose;
    public GameObject menuToOpen;

    public void HowToPlay()
    {
        menuToClose.SetActive(false);
        menuToOpen.SetActive(true);
    }
}
