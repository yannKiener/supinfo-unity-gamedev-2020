using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class GameUtils 
{
    private static Score score;
    private static string highScoreFilePath = Application.persistentDataPath + "/highscores.save";
    private static GameObject player;

    private static int shipKillCount = 0;
    private static int enemyGroundKillCount = 0;
    private static int friendlyGroundKillCount = 0;
    private static int friendlySavedCount = 0;

    public static void AddShipKill()
    {
        shipKillCount += 1;
    }

    public static void AddEnemyGroundKill()
    {
        enemyGroundKillCount += 1;
    }

    public static void AddFriendlyGroundKill()
    {
        friendlyGroundKillCount += 1;
    }

    public static void AddFriendlySaved()
    {
        friendlySavedCount += 1;
    }

    public static int GetShipKilled()
    {
        return shipKillCount;
    }

    public static int GetEnemyGroundKilled()
    {
        return enemyGroundKillCount;
    }

    public static int GetFriendlyKilled()
    {
        return friendlyGroundKillCount;
    }

    public static int GetFriendlySaved()
    {
        return friendlySavedCount;
    }

    public static bool IsPlayerAlive()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        return player != null && player.activeSelf && player.GetComponent<Unit>() != null && player.GetComponent<Unit>().IsAlive();
    }

    public static GameObject GetPlayerGameObject()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        return player;
    }

    //Setting player info
    public static void SetPlayerScoreTo(string playerName, int scoreCount)
    {
        if(scoreCount == 0)
        {
            enemyGroundKillCount = 0;
            friendlyGroundKillCount = 0;
            shipKillCount = 0;
        }
        score = new Score(playerName, scoreCount);
    }

    public static Score GetScore()
    {
        if(score == null)
        {
            score = new Score("Testing", 0);
        }
        return score;
    }

    public static void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    //Saves a new HighScore
    public static void SaveScore(Score score)
    {
        HighScores highScores = LoadHighScores();
        highScores.AddScore(score);
        SaveHighScores(highScores);
    }

    private static void SaveHighScores(HighScores highScores)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(highScoreFilePath);
        Debug.Log("Saving highscores at : " + highScoreFilePath);
        bf.Serialize(file, highScores);
        file.Close();
    }

    //Read and return HighScores
    public static HighScores LoadHighScores()
    {
        HighScores scoreList = null;
        if (File.Exists(highScoreFilePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(highScoreFilePath, FileMode.Open);
            scoreList = (HighScores)bf.Deserialize(file);
            file.Close();
        } else
        {
            Debug.Log("Save file " + highScoreFilePath + " not found, using stub");
            scoreList = LoadFakeScores();
        }

        return scoreList;
    }

    private static HighScores LoadFakeScores()
    {
        Debug.Log("Supposed to be called once (When no highscore file is found)");
        Score score1 = new Score("Andrew", 2100);
        Score score2 = new Score("RogueLeader", 1730);
        Score score3 = new Score("Mike", 100);

        Dictionary<int, Score> highScoreList = new Dictionary<int, Score>();
        highScoreList.Add(1, score1);
        highScoreList.Add(2, score2);
        highScoreList.Add(3, score3);

        HighScores highScores = new HighScores(highScoreList);
        
        return highScores;
    }

}
