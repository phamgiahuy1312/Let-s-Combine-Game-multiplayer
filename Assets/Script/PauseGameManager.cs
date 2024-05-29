using Photon.Pun;
using UnityEngine;

public class PauseGameManager : MonoBehaviourPunCallbacks
{
    public GameObject pausePanel;
    public GameObject exitButton;
    public GameObject waitingText;
    private bool isPaused = false;

    private void Start()
    {
        pausePanel.SetActive(false);
        waitingText.SetActive(false);
        exitButton.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        exitButton.SetActive(true);
        waitingText.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("PauseGameForOtherPlayer", RpcTarget.Others);
        }
        else
        {
            photonView.RPC("PauseGameForOtherPlayerFromClient", RpcTarget.Others);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        exitButton.SetActive(false);
        waitingText.SetActive(false);
    }

    public void OnClickExit()
    {
        ExitGame();
    }

    [PunRPC]
    void PauseGameForOtherPlayer()
    {
        Time.timeScale = 0f;
        isPaused = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            pausePanel.SetActive(true);
            exitButton.SetActive(false);
            waitingText.SetActive(true);
        }
    }

    [PunRPC]
    void PauseGameForOtherPlayerFromClient()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Time.timeScale = 0f;
            isPaused = true;
            pausePanel.SetActive(true);
            exitButton.SetActive(false);
            waitingText.SetActive(true);
        }
    }

    [PunRPC]
    void ExitGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Main Menu");
    }
}
