using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomList : MonoBehaviourPunCallbacks
{
    public static RoomList Instance;
    [Header("UI")] public Transform roomListParent;
    public GameObject roomListItemPrefab;
    [Header("UI")] public Transform roomListParent1;
    public GameObject roomListItemPrefab1;

    public GameObject mainMenuObject;
    public MainMenu MainMenu;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private void Awake()
    {
        Instance = this;
    }
    
    IEnumerator Start()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        yield return new WaitUntil(() => !PhotonNetwork.InRoom);

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            yield return new WaitUntil(() => !PhotonNetwork.IsConnected);
        }
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        // PhotonNetwork.JoinLobby();
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            int index = cachedRoomList.FindIndex(r => r.Name == room.Name);

            if (room.RemovedFromList)
            {
                if (index != -1)
                {
                    cachedRoomList.RemoveAt(index);
                }
            }
            else
            {
                if (index == -1)
                {
                    cachedRoomList.Add(room);
                }
                else
                {
                    cachedRoomList[index] = room;
                }
            }
        }

        UpdateUI();
    }



    void UpdateUI()
    {
        foreach(Transform roomItem in roomListParent)
        {
            Destroy(roomItem.gameObject);
        }


        foreach (var room in cachedRoomList)
        {
            GameObject roomItem = Instantiate(roomListItemPrefab, roomListParent);

            roomItem.transform.GetChild(0).GetComponent<Text>().text = room.Name;

            roomItem.transform.GetChild(1).GetComponent<Text>().text = room.PlayerCount + "/2";

            roomItem.GetComponent<RoomItemButton>().roomName = room.Name;

            RoomItemButton buttonScript = roomItem.GetComponent<RoomItemButton>();
            buttonScript.roomName = room.Name;

        }
    }
    

    public void clickRoomItem(string _roomName)
    {
        MainMenu.RoomNameJoin = _roomName;
        mainMenuObject.SetActive(true);
    }
}
