using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    //new
    public int nextPlayerteam;
    public Transform[] spawnPointsTeamOne;
    public Transform[] spawnPointsTeamTwo;

    public static int blueScore = 0;
    public static int redScore = 0;
    public Text blueScoreText;
    public Text redScoreText;

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

    private void Update()
    {
        SetScoreText();
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
}
