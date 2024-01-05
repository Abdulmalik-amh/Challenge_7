using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using Unity.VisualScripting;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    //new
    public int nextPlayerteam;
    public Transform[] spawnPointsTeamOne;
    public Transform[] spawnPointsTeamTwo;

    public Text messageText;

    public static int blueScore = 0;
    public static int redScore = 0;
    public Text blueScoreText;
    public Text redScoreText;


    public static readonly byte RestartGamEventCode = 1;

    private void Awake()
    {
        SetScoreText();
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(DisplayMessage("Fight"));
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded (Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1)
        {
            PhotonNetwork.Instantiate(Path.Combine("PlayerControllerManger"), Vector3.one, Quaternion.identity);
        }
    }


    IEnumerator DisplayMessage(string message)
    {
        messageText.text = message;
        yield return new WaitForSeconds(4);
        messageText.text = "";
    }
    private void Update()
    {
        SetScoreText();

        //Check score and win
        if (redScore >= 3)
        {
            StartCoroutine(DisplayMessage("Red Team Win !!"));
            //if master client send restart event
            if (PhotonNetwork.IsMasterClient)
            {
               StartCoroutine(RestartGame());
            }
        }

        if (blueScore >= 3)
        {
            StartCoroutine(DisplayMessage("Blue Team Win !!"));
            //if master client send restart event
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(RestartGame());
            }
        }
    }
    public void uodateTeam()
    {
        if (nextPlayerteam == 1)
        {
            nextPlayerteam = 2;
        }
        else
        {
            nextPlayerteam = 1;
        }
    }

   public void SetScoreText()
    {
        blueScoreText.text = blueScore.ToString();
        redScoreText.text = redScore.ToString();
    }


    IEnumerator RestartGame()
    {
        redScore = 0;
        blueScore = 0;
        yield return new WaitForSeconds(3);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        ExitGames.Client.Photon.SendOptions sendOption = new ExitGames.Client.Photon.SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(RestartGamEventCode,null,raiseEventOptions, sendOption);
    }
}
