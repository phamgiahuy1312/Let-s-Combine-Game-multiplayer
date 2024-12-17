using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoom_List : MonoBehaviour
{
   public string roomName;

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(roomName))
    {
        Debug.LogError("Room name is empty! Cannot join room.");
        return;
    }

    Debug.Log("Trying to join room: " + roomName);
    RoomList.Instance.JoinRoom(roomName);
    }
}
