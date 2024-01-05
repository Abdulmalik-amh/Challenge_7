using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class PlayerControllerManager : MonoBehaviour
{
   public PhotonView view;
    public GameObject myPlayer;
    public int myTeam;

    int kills;
    int deaths;

    public static PlayerControllerManager instance;


    private void Awake()
    {
        instance = this;
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (view.IsMine)
        {
            
            view.RPC("RPC_GetTeam", RpcTarget.MasterClient);
        }

        PhotonNetwork.LocalPlayer.CustomProperties["Team"] = myTeam;
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
                        RoomManager.Instance.spawnPointsTeamOne[spawnPicker].rotation, 0, new object[] { view.ViewID });
                }


            }

            else
            {
                int spawnPicker = Random.Range(0, RoomManager.Instance.spawnPointsTeamTwo.Length);
                if (view.IsMine) 
                {
                myPlayer =  PhotonNetwork.Instantiate(Path.Combine("Player1"), RoomManager.Instance.spawnPointsTeamTwo[spawnPicker].position,
                    RoomManager.Instance.spawnPointsTeamTwo[spawnPicker].rotation, 0, new object[] {view.ViewID});
                }
            }
        
    }

    public void Die()
    {
        PhotonNetwork.Destroy(myPlayer);
        CreatController();

        deaths++;

        Hashtable hash = new Hashtable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public void GetKill()
    {
        view.RPC(nameof(RPC_GetKill), view.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;

        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

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

    public static PlayerControllerManager Find(Player player)
	{
		return FindObjectsOfType<PlayerControllerManager>().SingleOrDefault(x => x.view.Owner == player);
	}
}
