using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
using static Photon.Pun.UtilityScripts.PunTeams;

public class PlayerControllerManager : MonoBehaviourPunCallbacks
{
   public PhotonView view;
    public GameObject myPlayer;
    public int myTeam;

   public  int kills;
    public int deaths;

    public static PlayerControllerManager instance;

    private Dictionary<int, int> myTeams = new Dictionary<int, int>();


    private void Awake()
    {
        instance = this;
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (view.IsMine)
        {
            
            //view.RPC("RPC_GetTeam", RpcTarget.MasterClient);
        }

       // PhotonNetwork.LocalPlayer.CustomProperties["Team"] = myTeam;

    }

    private void Update()
    {
        if (myPlayer == null)
        {
            CreatController();
        }
    }

    public void CreatController()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            //PhotonNetwork.LocalPlayer.CustomProperties["Team"] = myTeam;
            myTeam = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            
        }

        AssignPlayerToSpownArea(myTeam);

    }

    void AssignPlayerToSpownArea(int team)
    {

        if (myTeam == 1)
        {
            int spawnPicker = Random.Range(0, RoomManager.Instance.spawnPointsTeamOne.Length);
            if (view.IsMine)
            {
                myPlayer = PhotonNetwork.Instantiate(Path.Combine("PlayerRed"), RoomManager.Instance.spawnPointsTeamOne[spawnPicker].position,
                        RoomManager.Instance.spawnPointsTeamOne[spawnPicker].rotation, 0, new object[] { view.ViewID });
            }


        }

        else
        {
            int spawnPicker = Random.Range(0, RoomManager.Instance.spawnPointsTeamTwo.Length);
            if (view.IsMine)
            {
                myPlayer = PhotonNetwork.Instantiate(Path.Combine("PlayerBlue"), RoomManager.Instance.spawnPointsTeamTwo[spawnPicker].position,
                    RoomManager.Instance.spawnPointsTeamTwo[spawnPicker].rotation, 0, new object[] { view.ViewID });
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

    //[PunRPC]
    //void RPC_GetTeam()
    //{
    //    myTeam = RoomManager.Instance.nextPlayerteam;
    //    RoomManager.Instance.uodateTeam();
    //    view.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
    //}

    //[PunRPC]
    //void RPC_SentTeam(int whichteam)
    //{
    //    myTeam = whichteam;
    //}

    void AssignTeamToAllPlayers()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("Team"))
            {
                int team = (int)player.CustomProperties["Team"];
                myTeams[player.ActorNumber] = team;
                Debug.Log(player.NickName + "team n: " + team);


            }
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        AssignTeamToAllPlayers();
    }

    public static PlayerControllerManager Find(Player player)
	{
		return FindObjectsOfType<PlayerControllerManager>().SingleOrDefault(x => x.view.Owner == player);
	}
}
