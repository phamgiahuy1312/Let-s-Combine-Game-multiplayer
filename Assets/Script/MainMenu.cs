using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    //[SerializeField] GameObject panel;
    [SerializeField] GameObject highScorePanel;
    public InputField nameInput;
    private string roomName;
    // Gọi hàm này khi thay đổi tên người chơi
    public void ChangeName()
    {
        PhotonNetwork.NickName = nameInput.text;
    }

    // Gọi hàm này khi muốn tạo phòng mới
    public void CreateRoom()
    {
        // Kiểm tra xem đã rời khỏi phòng (nếu đang ở trong phòng) và lobby chưa
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        else if (PhotonNetwork.IsConnectedAndReady)
        {
            // Tạo phòng mới khi đã ở trên Master Server
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        }
        else
        {
            Debug.LogError("Not connected to Master Server. Wait for OnConnectedToMaster callback before creating room.");
        }
    }

    // Gọi hàm này khi muốn tham gia vào phòng
    public void JoinRoom()
    {
        // Kiểm tra xem đã rời khỏi phòng (nếu đang ở trong phòng) và lobby chưa
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        else
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
    }

    // Gọi khi đã tham gia vào phòng thành công
    public override void OnJoinedRoom()
    {
        roomName = PhotonNetwork.CurrentRoom.Name;
        PhotonNetwork.LoadLevel("Game");
    }

    // Gọi khi kết nối đến Master Server thành công
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server.");
    }

    // Gọi khi mất kết nối với Master Server
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Disconnected from Master Server. Reason: {0}", cause);
    }

    // Gọi khi người chơi nhấn vào nút HighScore
    public void OnClickHighScore()
    {
        highScorePanel.SetActive(true);
    }

    // Gọi khi người chơi nhấn vào nút ClosePanel
    public void OnClickClosePanel()
    {
        highScorePanel.SetActive(false);
    }
}
