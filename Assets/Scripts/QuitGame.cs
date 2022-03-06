using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class QuitGame : MonoBehaviourPunCallbacks
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                PhotonNetwork.LeaveRoom();
                Debug.Log("Leaving Room...");
            }
            if(SceneManager.GetActiveScene().name == "ConnectScene")
            {
                PhotonNetwork.Disconnect();
                Debug.Log("Leaving Lobby");
            }
            
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("WelcomeScreen");
    }
    public void MultiplayerReturnToMainMenu()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("Leaving Room...");
        }
        if (SceneManager.GetActiveScene().name == "ConnectScene")
        {
            PhotonNetwork.Disconnect();
            Debug.Log("Leaving Lobby");
        }
    }
    public override void OnLeftRoom()
    { 
       
        SceneManager.LoadScene("WelcomeScreen");
    }
    public override void OnLeftLobby()
    {
        SceneManager.LoadScene("WelcomeScreen");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("WelcomeScreen");
    }

}
