using UnityEngine;

public class RoomItemButton : MonoBehaviour
{
    public string roomName;
    public static GameObject inputName_Panel; // Shared panel
    public GameObject inputName_PanelPrefab; // Assign the prefab in the Inspector

    public void OnButtonPressed()
    {
        if (inputName_Panel == null)
        {
            // Instantiate the panel once
            inputName_Panel = Instantiate(inputName_PanelPrefab, transform.parent.parent); // Adjust parent
        }

        // Show the shared panel
        inputName_Panel.SetActive(true);
    }

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
