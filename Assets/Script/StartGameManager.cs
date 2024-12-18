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

    public GameObject headerUI;
    private Vector3 headerUIPos;
    AudioManagers audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagers>();
    } 
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
        // headerUIPos = headerUI.transform.position;
        // print(headerUIPos);
        audioManager.PlaySFX(audioManager.Touch);
        //check room have 2 player
            BoxChat.SetActive(false);
            chatBoxButton.gameObject.SetActive(false);
            photonView.RPC("StartGame", RpcTarget.All);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
        }

    }

    public void OnClickchatBoxButton()
    {
        audioManager.PlaySFX(audioManager.Touch);
        BoxChat.SetActive(true);
    }
    public void OnClickCloseChatBoxButton()
    {   
        audioManager.PlaySFX(audioManager.Touch);
        BoxChat.SetActive(false);
    }

    [PunRPC]
    public void StartGame()
    {
        AnimationUI_IN(headerUI,0,-50, 0);
        spawner.StartGame();
        chatBoxButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false); 
        BoxChat.SetActive(false); 
        WaitingHostStart.gameObject.SetActive(false);
    }

        private void AnimationUI_OUT(GameObject Panel,int x,int y,int z){
        LeanTween.moveLocal(Panel, new Vector3(x,y,z),1f).setEase(LeanTweenType.easeInBack);

    }

     private void AnimationUI_IN(GameObject Panel,int x,int y,int z){
        LeanTween.moveLocal(Panel, new Vector3(x,y,z),1f).setEase(LeanTweenType.easeOutBack);

    }

}
