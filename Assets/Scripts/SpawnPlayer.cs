using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerPrefab;
    [SerializeField]
    private Transform[] PlayerTransform;

    public GameObject[] Players;

    bool OppositePlayerDestroyed;
    int numberPlayers;
    private bool moved;


    private void Start()
    {
        moved = false;
        OppositePlayerDestroyed = false;
        if(PhotonNetwork.PlayerList.Length == 1)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, PlayerTransform[0].position, PlayerTransform[0].rotation);
        }
        else if(PhotonNetwork.PlayerList.Length == 2)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, PlayerTransform[1].position, PlayerTransform[1].rotation);
        }


    }
    private void Update()
    {
        /*Players = GameObject.FindGameObjectsWithTag("PlayerHolder");
        Debug.Log(Players.Length);
        if(Players.Length > 1 )
        {
            Players[1].transform.position = new Vector3(Players[1].transform.position.x, Players[1].transform.position.y, PlayerTransform[1].position.z);
            Players[1].transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            PhotonView PVO = Players[1].GetComponent<PhotonView>();
            PhotonView PVT = Players[0].GetComponent<PhotonView>();

            if (PVO.IsMine)
            {
                if (Players[0].GetComponentInChildren<Camera>() != null)
                    Destroy(Players[0].GetComponentInChildren<Camera>().gameObject);
            }
            else if(PVT.IsMine)
            {
                if (Players[1].GetComponentInChildren<Camera>() != null)
                    Destroy(Players[1].GetComponentInChildren<Camera>().gameObject);
            }

            if (PVT.IsMine)
                return;
            Destroy(Players[0].GetComponentInChildren<Camera>().gameObject);
        }*/
    }
}
