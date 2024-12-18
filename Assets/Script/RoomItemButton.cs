using UnityEngine;

public class RoomItemButton : MonoBehaviour
{
    public string roomName;
    public void JoinRoom()
    {

        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogError("Room name is empty! Cannot join room.");
            return;
        }

        RoomList.Instance.clickRoomItem(roomName);
    }

}
