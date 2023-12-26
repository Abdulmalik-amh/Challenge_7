using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerUsernameManger : MonoBehaviour
{
    [SerializeField] private InputField usernameInput;
    [SerializeField] private Text errorMessegeText;
    

    private void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            usernameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");

        }
    }
    public void PlayerusernameInputValueChange()
    {
        string username = usernameInput.text;

        if(!string.IsNullOrEmpty(username) && username.Length <= 20)
        {
            PhotonNetwork.NickName = username;
            PlayerPrefs.SetString("username", username);
            errorMessegeText.text = "";
            MenuManager.instance.OpenMenu("StartMenu");
        }
        else
        {
            errorMessegeText.text = "username must not be empty";
        }
    }
}
