using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameResumeButton : MonoBehaviour
{
    public void Resume()
    {
        GameObject.Find("Main Camera").GetComponent<GameController>().ToggleInGameMenu();
    }
}
