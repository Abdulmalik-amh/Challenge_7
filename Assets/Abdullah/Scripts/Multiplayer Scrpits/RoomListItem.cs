using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] Text roomNameText;
    [SerializeField] Text roomNum;

    public RoomInfo info;
    public RoomInfo PInfo;
    //RoomInfo info2;

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        roomNameText.text = _info.Name;

        PInfo = _info;

        roomNum.text = _info.PlayerCount + "/4"; 
        
    }


    //public void SetCount(RoomInfo _info2)
    //{
    //    //info2 = _info2;
    //    roomNum.text = _info2.Name + "/6";

    //}


    public void OnClick()
    {
       Launcher.Instance.JoinRoom(info);
    }
}
