using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingHighScores : MonoBehaviour
{
    public float slideSpeed = 50;
    public float resetLimit = 600;
    private Vector3 startPosition;
    private HighScores highScores;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        startPosition = this.transform.position;
        highScores = LoadScores();
        text = gameObject.GetComponent<Text>();
        text.text = highScores.GetPrettyString();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, Time.deltaTime * slideSpeed);

        if (IsOutOfPosition())
        {
            ResetPosition();
        }
    }

    private bool IsOutOfPosition()
    {
        return transform.localPosition.y > resetLimit;
    }

    private void ResetPosition()
    {
        this.transform.position = startPosition;
    }

    private HighScores LoadScores()
    {
        return GameUtils.LoadHighScores();
    }
}
