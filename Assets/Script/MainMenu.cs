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

    [SerializeField] GameObject quitGamePanel;
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

    






//*********** ON OFF HIGHSCORE PANEL ***********//
    public void OnClickHighScore()
    {
        audioManager.PlaySFX(audioManager.Touch);
        highScorePanel.SetActive(true);
    }

    public void OnClickClosePanel()
    {
        audioManager.PlaySFX(audioManager.Touch);
        highScorePanel.SetActive(false);
    }
//***********************************************//

//*********** ON OFF SETTING PANEL ***********//
    public void OpenSetting()
    {
        audioManager.PlaySFX(audioManager.Touch);
        SettingPanel.SetActive(true);
        AnimationUI_IN(SettingPanel,0,0,0);

    }
    public void CloseSetting()
    {
        audioManager.PlaySFX(audioManager.Touch);
        AnimationUI_OUT(SettingPanel,0,1000,0);
    }
//***********************************************//

//*********** ON OFF LIST ROOM PANEL ***********//

    public void OpenListRoom()
    { 
        audioManager.PlaySFX(audioManager.Touch);
        listRoomPanel.SetActive(true);
        AnimationUI_IN(listRoomPanel,0,0,0);

    }
    public void CloseListRoom()
    {
        audioManager.PlaySFX(audioManager.Touch);
        AnimationUI_OUT(listRoomPanel,0,1000,0);
    }
 //***********************************************//

 //*********** ON OFF QUIT GAME PANEL ***********//
    public void OpenQuitGame()
    {
        audioManager.PlaySFX(audioManager.Touch);
        quitGamePanel.SetActive(true);
        AnimationUI_IN(quitGamePanel,0,0,0);

    }
    public void CloseQuitGame()
    {
        audioManager.PlaySFX(audioManager.Touch);
        AnimationUI_OUT(quitGamePanel,0,1000,0);
    }
//***********************************************//
    public void QuitGame()
    {
        audioManager.PlaySFX(audioManager.Touch);
        Application.Quit();
    }




    private void AnimationUI_OUT(GameObject Panel,int x,int y,int z){
        LeanTween.moveLocal(Panel, new Vector3(x,y,z),1f).setEase(LeanTweenType.easeInBack);

    }

     private void AnimationUI_IN(GameObject Panel,int x,int y,int z){
        LeanTween.moveLocal(Panel, new Vector3(x,y,z),1f).setEase(LeanTweenType.easeOutBack);

    }



}
