using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    public Text playerUsername;
    public Text teamNumb;

    Player player;

    int team;
    public void SetUp(Player _player, int _team)
    {
        player = _player;
        team = _team;
        playerUsername.text = _player.NickName;
       teamNumb.text = "T" + _team;

        ExitGames.Client.Photon.Hashtable customprops = new ExitGames.Client.Photon.Hashtable();
        customprops["Team"] = _team;
        player.SetCustomProperties(customprops);

        if (_team == 1)
        {
            teamNumb.color = Color.red;
        }

        if (_team == 2)
        {
            teamNumb.color = Color.blue;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
