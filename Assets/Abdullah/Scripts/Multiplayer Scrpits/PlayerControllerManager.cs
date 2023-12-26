using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

public class PlayerControllerManager : MonoBehaviour
{
   public PhotonView view;
    public GameObject myPlayer;
    public int myTeam;
 

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (view.IsMine)
        {
            
            view.RPC("RPC_GetTeam", RpcTarget.MasterClient);
        }
        
    }

    private void Update()
    {
        if (myPlayer == null &&  myTeam != 0)
        {
            CreatController();
        }
    }

    void CreatController()
    {
            if (myTeam == 1)
            {
                int spawnPicker = Random.Range(0, RoomManager.Instance.spawnPointsTeamOne.Length);
                if (view.IsMine)
                {
                myPlayer =  PhotonNetwork.Instantiate(Path.Combine("Player"), RoomManager.Instance.spawnPointsTeamOne[spawnPicker].position,
                        RoomManager.Instance.spawnPointsTeamOne[spawnPicker].rotation, 0);
                }
            }

            else
            {
                int spawnPicker = Random.Range(0, RoomManager.Instance.spawnPointsTeamTwo.Length);
                if (view.IsMine) 
                {
                myPlayer =  PhotonNetwork.Instantiate(Path.Combine("Player1"), RoomManager.Instance.spawnPointsTeamTwo[spawnPicker].position,
                    RoomManager.Instance.spawnPointsTeamTwo[spawnPicker].rotation, 0);
                }
            }
        
    }

    [PunRPC]
    void RPC_GetTeam()
    {
        myTeam = RoomManager.Instance.nextPlayerteam;
        RoomManager.Instance.uodateTeam();
        view.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
    }

    [PunRPC]
    void RPC_SentTeam(int whichteam)
    {
        myTeam = whichteam;
    }
}
