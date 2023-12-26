using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;
using JetBrains.Annotations;
using Photon.Pun.Demo.Cockpit;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] InputField roomNameInputField;
    [SerializeField] Text roomNameText;
    [SerializeField] Text errorText;
    [SerializeField] Transform roomListContant;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContant;
    [SerializeField] GameObject playerListItemPrefab;
    public GameObject startButton;
    public GameObject LeaveButton;
    public GameObject countToStart;
    public Text errorTextToJoinRoom;
    public Animator anim;


    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log("Connecting To Master");
        PhotonNetwork.ConnectUsingSettings();
        countToStart.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {

        Debug.Log("Connected To Master");

        PhotonNetwork.JoinLobby();
        
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.instance.OpenMenu("UsernameMenu");

        Debug.Log("Joined Lobby");
    }


    public void onBack()
    {
        MenuManager.instance.OpenMenu("StartMenu");
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        RoomOptions roomObs = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomNameInputField.text , roomObs);

        MenuManager.instance.OpenMenu("LoadingScreen");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.instance.OpenMenu("RoomMenu");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name  +"'s Room";
   

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContant)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++) 
        {
            Instantiate(playerListItemPrefab, playerListContant).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startButton.SetActive(PhotonNetwork.IsMasterClient);
  
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string errorMessage)
    {
        errorText.text = "Room Generation Unsuccesfull" + errorMessage;
        MenuManager.instance.OpenMenu("ErrorMenu"); 
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        MenuManager.instance.OpenMenu("FindRoomMenu");
        errorTextToJoinRoom.text = "Room is Full currently choose another Room or create a new one ";
        StartCoroutine(errorTextToJoiningRoom());
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("LoadingScreen");
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCount());
 
    }

    public void QuitGame()
    {
        Application.Quit();

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("LoadingScreen");
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("StartMenu");
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContant)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
                Instantiate(roomListItemPrefab, roomListContant).GetComponent<RoomListItem>().SetUp(roomList[i]);

        }

        //anim.enabled = true;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContant).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    IEnumerator  StartGameCount()
    {
        startButton.SetActive(false);
        LeaveButton.SetActive(false);
        countToStart.SetActive(true);
        //anim.SetBool("count",true);
        yield return new WaitForSeconds(3f);
        PhotonNetwork.LoadLevel(1);

    }

    IEnumerator errorTextToJoiningRoom()
    {
        
        yield return new WaitForSeconds(9);
        errorTextToJoinRoom.text = "";
    }


}
