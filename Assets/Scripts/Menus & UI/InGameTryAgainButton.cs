using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTryAgainButton : MonoBehaviour
{
    public void TryAgain()
    {
        //Reset player score (not affected by reloading since GameUtils is static )
        GameUtils.SetPlayerScoreTo(GameUtils.GetScore().GetName(), 0);
        Time.timeScale = 1;
        GameUtils.ReloadScene();
    }
}
