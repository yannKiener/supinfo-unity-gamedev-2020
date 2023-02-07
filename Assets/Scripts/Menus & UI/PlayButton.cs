using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{

    public GameObject inputText;

    public void Play()
    {
        string playerName = inputText.GetComponent<Text>().text;
        if (playerName != null && playerName.Length > 1)
        {
            GameUtils.SetPlayerScoreTo(playerName, 0);
            Time.timeScale = 1;
            GameUtils.LoadScene("LevelOne");
            Debug.Log("GL HF ! ");
        } else
        {
            Debug.Log("Choose a name !");
        }
    }
}
