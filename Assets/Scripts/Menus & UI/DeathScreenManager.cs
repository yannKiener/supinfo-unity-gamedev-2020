using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{

    public GameObject deathMenu;
    public float fadeInTime;

    public GameObject playerText;
    public GameObject scoreText;
    public GameObject shipText;
    public GameObject enemyText;
    public GameObject friendlyProtectedText;
    public GameObject friendlyKIAText;

    public List<AudioClip> gameOverMusics;
    public GameObject musicManagerGameObject;


    
    private void OnEnable()
    {
        StartCoroutine(FadeInImage(gameObject.GetComponent<Image>()));
        StartCoroutine(ShowMenuAfterFadeIn());
        musicManagerGameObject.GetComponent<MusicManager>().SetNewPlaylist(gameOverMusics);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeInImage(Image img)
    {
        for (float i = 0; i <= fadeInTime; i += Time.deltaTime / fadeInTime)
        {
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    IEnumerator ShowMenuAfterFadeIn()
    {
        yield return new WaitForSeconds(fadeInTime);
        //Stop everything
        Time.timeScale = 0;
        deathMenu.SetActive(true);
        Score actualScore = GameUtils.GetScore();
        playerText.GetComponent<Text>().text = actualScore.GetName();
        scoreText.GetComponent<Text>().text = "Score : " + actualScore.GetCount();
        shipText.GetComponent<Text>().text = "Ship killed : " + GameUtils.GetShipKilled();
        enemyText.GetComponent<Text>().text = "Rebels killed : " + GameUtils.GetEnemyGroundKilled();
        friendlyProtectedText.GetComponent<Text>().text = "Allies protected : " + GameUtils.GetFriendlySaved();
        friendlyKIAText.GetComponent<Text>().text = "Allies fallen : " + GameUtils.GetFriendlyKilled();
        GameUtils.SaveScore(actualScore);
    }
}
