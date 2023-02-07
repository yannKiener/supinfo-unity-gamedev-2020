using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score 
{
    string name;
    int count;

    public Score(string name, int count)
    {
        this.name = name;
        this.count = count;
    }

    public int GetCount()
    {
        return count;
    }

    public string GetName()
    {
        return name;
    }

    public void Addcount(int points)
    {
        this.count += points;
    }

}
