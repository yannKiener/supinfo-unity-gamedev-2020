using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("See you soon !");
        Application.Quit();
    }
}
