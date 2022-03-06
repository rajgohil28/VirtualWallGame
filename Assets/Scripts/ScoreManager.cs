using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
public class ScoreManager : MonoBehaviour
{
    public static int m_ScoreOne = 0;
    public int ScoreOne { get {return m_ScoreOne; } }

    public static int m_ScoreTwo = 0;
    public int ScoreTwo { get { return m_ScoreTwo; } }

    private TextMeshProUGUI scoreOneText;
    private TextMeshProUGUI scoreTwoText;

    private void Start()
    {
        scoreOneText = GameObject.Find("Player1Score").GetComponent<TextMeshProUGUI>();
        scoreTwoText = GameObject.Find("Player2Score").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        //scoreText.text = m_Score.ToString();
        if(PhotonNetwork.PlayerList.Length == 2)
        {
            scoreOneText.text = PhotonNetwork.PlayerList[0].GetScore().ToString();
            scoreTwoText.text = PhotonNetwork.PlayerList[1].GetScore().ToString();
        }     
    }

    public void Reset()
    {
        m_ScoreOne = 0;
        m_ScoreTwo = 0;
        scoreOneText.text = m_ScoreOne.ToString();
        scoreTwoText.text = m_ScoreTwo.ToString();
    }

    public void IncreaseScorePlayerOne()
    {
        m_ScoreOne += 1;
        PhotonNetwork.PlayerList[0].AddScore(1);
        scoreOneText.text = PhotonNetwork.PlayerList[0].GetScore().ToString();
        Debug.Log("Player One Scored!");
    }
    public void IncreaseScorePlayerTwo()
    {
        m_ScoreTwo += 1;
        PhotonNetwork.PlayerList[1].AddScore(1);
        scoreTwoText.text = PhotonNetwork.PlayerList[1].GetScore().ToString();
        Debug.Log("Player Two Scored!");
    }
}
