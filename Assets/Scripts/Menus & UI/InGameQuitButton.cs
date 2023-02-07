using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameQuitButton : MonoBehaviour
{
    public void ExitGame()
    {
        GameUtils.LoadScene("MainMenu");
    }
}
