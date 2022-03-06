using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSinglePlayer
{

    private GameObject m_gameObject;

    private static GameManagerSinglePlayer m_Instance;
    public static int WebCamIndex;
    public static GameManagerSinglePlayer Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameManagerSinglePlayer();
                m_Instance.m_gameObject = new GameObject("_gameManagerSinglePlayer");
                m_Instance.m_gameObject.AddComponent<ScoreManagerSinglePlayer>();

            }
            return m_Instance;
        }
    }

    private ScoreManagerSinglePlayer m_ScoreManager;
    public ScoreManagerSinglePlayer ScoreManager
    {
        get
        {
            if (m_ScoreManager == null)
                m_ScoreManager = m_gameObject.GetComponent<ScoreManagerSinglePlayer>();
            return m_ScoreManager;
        }
    }
}
