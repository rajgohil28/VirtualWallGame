using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManagerSinglePlayer : MonoBehaviour
{
    private int m_Score = 0;
    public int Score { get { return m_Score; } }

    private int m_Lives = 3;
    public int Lives { get { return m_Lives; } }

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI livesText;

    private void Start()
    {
        scoreText = GameObject.Find("DisplayScore").GetComponent<TextMeshProUGUI>();
        livesText = GameObject.Find("DisplayLives").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        //scoreText.text = m_Score.ToString();
    }

    public void Reset()
    {
        m_Score = 0;
        m_Lives = 3;
        scoreText.text = m_Score.ToString();
        livesText.text = m_Lives.ToString() + "/3";
    }

    public void IncreaseScore()
    {
        m_Score += 1;
        scoreText.text = m_Score.ToString();
    }
    public void DecreaseLive()
    {
        if (m_Lives > 0)
            m_Lives--;

        livesText.text = m_Lives.ToString() + "/3";
    }


}
