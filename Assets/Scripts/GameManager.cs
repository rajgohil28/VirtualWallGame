using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{

    private GameObject m_gameObject;

    private static GameManager m_Instance;
    public static int WebCamIndex;
    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameManager();
                m_Instance.m_gameObject = new GameObject("_gameManager");
                m_Instance.m_gameObject.AddComponent<ScoreManager>();
        
            }
            return m_Instance;
        }
    }

    private ScoreManager m_ScoreManager;
    public ScoreManager ScoreManager
    {
        get
        {
            if (m_ScoreManager == null)
                m_ScoreManager = m_gameObject.GetComponent<ScoreManager>();
            return m_ScoreManager;
        }
    }
}
