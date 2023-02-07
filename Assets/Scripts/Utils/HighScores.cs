using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class HighScores 
{
    Dictionary<int, Score> highScores;

    public HighScores(Dictionary<int, Score> highScores)
    {
        this.highScores = highScores;
    }

    public void AddScore(Score score)
    {
        int i = 0;
        foreach (KeyValuePair<int,Score> kv in highScores)
        {
            i++;
            if (kv.Value.GetCount() < score.GetCount())
            {
                //Debug.Log("Pushing score at position : " + kv.Key);
                pushScore(kv.Key, score);
                return;
            }
        }
        //If no HighScore is beaten and map isn't full (10), we add the score at the end.
        if (i < 10)
        {
            Debug.Log("New score at bottom.");
            highScores.Add(i + 1, score);
        }
    }

    //Used to "push down" lower existing scores
    private void pushScore(int position, Score score)
    {
        if (position < 10)
        {
            if (highScores.ContainsKey(position))
            {
                Score tempScore = highScores[position];
                highScores[position] = score;
                pushScore(position + 1, tempScore);
            } else
            {
                highScores[position] = score;
            }
        }
    }

    //Used to "pretty print" highscores
    public string GetPrettyString()
    {
        string result = "";

        foreach (KeyValuePair<int, Score> kv in highScores)
        {
            result += kv.Key + ". " + kv.Value.GetName() + "                                                                              " + kv.Value.GetCount() + "\r\n\r\n";
        }

        return result;
    }
}
