using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class StartGameManager : MonoBehaviourPunCallbacks
{
    public Button startButton;
    public GameObject BoxChat;
    public Spawner spawner;
    public Text WaitingHostStart;
    public Button chatBoxButton;
    public Button closeChatBoxBtn;
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(false);
            WaitingHostStart.gameObject.SetActive(true);
        }
        else
        {
            WaitingHostStart.gameObject.SetActive(false);
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
    }

    public void OnStartButtonClicked()
    {
        // Gửi RPC 
        BoxChat.SetActive(false);
        chatBoxButton.gameObject.SetActive(false);
        photonView.RPC("StartGame", RpcTarget.All);
    }

    public void OnClickchatBoxButton()
    {
        BoxChat.SetActive(true);
    }
    public void OnClickCloseChatBoxButton()
    {
        BoxChat.SetActive(false);
    }

    [PunRPC]
    public void StartGame()
    {
        spawner.StartGame();
        chatBoxButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false); 
        BoxChat.SetActive(false); 
        WaitingHostStart.gameObject.SetActive(false);
    }
}
