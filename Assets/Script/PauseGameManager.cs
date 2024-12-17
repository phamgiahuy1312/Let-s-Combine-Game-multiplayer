using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseGameManager : MonoBehaviourPunCallbacks
{
    public GameObject pausePanel; // Panel for pause UI
    public GameObject exitButton; // Exit button for initiator
    public GameObject resumeButton; // Resume button for initiator
    public GameObject waitingText; // Waiting message for non-initiator
    private bool isPaused = false;

    private void Start()
    {
        pausePanel.SetActive(false);
        exitButton.SetActive(false);    
        resumeButton.SetActive(false);
        waitingText.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;  
            NotifyKeyPress(isPaused); 
        }
    }

    void NotifyKeyPress(bool isPaused)
    {
        photonView.RPC("OnPlayerPressedEsc", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, isPaused);
    }

    public void OnClickResumeGame()
    {
        pausePanel.SetActive(false);
        exitButton.SetActive(false);
        resumeButton.SetActive(false);
        waitingText.SetActive(false);

        photonView.RPC("OnPlayerPressedEsc", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, false);
    }

    public void OnClickExit()
    {
        // Notify all players to leave the room
        photonView.RPC("OnExitGame", RpcTarget.All);
    }

    [PunRPC]
    void OnExitGame()
    {
        // Leave the Photon room
        PlayerStats.score = 0;
        PlayerStats.health = 15;
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        // Load the Main Menu after leaving the room
        PhotonNetwork.LoadLevel("Main Menu");
    }

    [PunRPC]
    public void OnPlayerPressedEsc(int actorNumber, bool isPaused)
    {
        this.isPaused = isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 0.01f;
            if (actorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                pausePanel.SetActive(true);
                exitButton.SetActive(true);
                resumeButton.SetActive(true);
                waitingText.SetActive(false);
            }    
            if(actorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                pausePanel.SetActive(true);
                exitButton.SetActive(false);
                resumeButton.SetActive(false);
                waitingText.SetActive(true);
            }
        }
        else
        {
                Time.timeScale = 1;
                PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 1;
                pausePanel.SetActive(false);
                exitButton.SetActive(false);
                resumeButton.SetActive(false);
                waitingText.SetActive(false);

        }

    }


}
