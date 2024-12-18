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
    [SerializeField] GameObject SettingPanel;
    [SerializeField] GameObject listRoomPanel;
    public InputField nameInput;
    public InputField nameInputTemp;
    private string roomName;
    public string RoomNameJoin = "RoomName";
    AudioManagers audioManager;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagers>();
    } 
        // Gọi hàm này khi thay đổi tên người chơi
    public void ChangeName()
    {
        PhotonNetwork.NickName = nameInput.text;
    }

    public void ChangeNameTemp(){
        PhotonNetwork.NickName = nameInputTemp.text;
    }

    // Gọi hàm này khi muốn tạo phòng mới
    public void CreateRoom()
    {
        audioManager.PlaySFX(audioManager.Touch);
        if (string.IsNullOrEmpty(createInput.text))
        {
            Debug.LogError("Room name is null or empty");
            return;
        }
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
        print("test");
        audioManager.PlaySFX(audioManager.Touch);
        if(!string.IsNullOrEmpty(joinInput.text))
        {
            RoomNameJoin = joinInput.text;
        }
        // check out room or lobby
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        // else if (PhotonNetwork.InLobby)
        // {
        //     PhotonNetwork.LeaveLobby();
        // }
        else
        {
            // RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 }; // Set room options
            PhotonNetwork.JoinRoom(RoomNameJoin); 
            print("Join room by name" + RoomNameJoin);
        }
    }

    // Gọi khi đã tham gia vào phòng thành công
    public override void OnJoinedRoom()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
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
        audioManager.PlaySFX(audioManager.Touch);
        highScorePanel.SetActive(true);
    }

    // Gọi khi người chơi nhấn vào nút ClosePanel
    public void OnClickClosePanel()
    {
        audioManager.PlaySFX(audioManager.Touch);
        highScorePanel.SetActive(false);
    }

    public void OpenSetting()
    {
        audioManager.PlaySFX(audioManager.Touch);
        SettingPanel.SetActive(true);
    }

    public void OpenListRoom()
    { 
        audioManager.PlaySFX(audioManager.Touch);
        listRoomPanel.SetActive(true);

    }
    public void CloseListRoom()
    {
        audioManager.PlaySFX(audioManager.Touch);
        listRoomPanel.SetActive(false);
    }

}
