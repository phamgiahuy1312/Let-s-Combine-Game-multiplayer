
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class Score : MonoBehaviour
{
    //public int score = 0;
    PhotonView view;
    public Text scoreDisplay;
    public GameObject gameOver;
    private Spawner spawner;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        //PlayerStats.score = 0;
        scoreDisplay.text = PlayerStats.score.ToString();
    }

    public void AddScore()
    {
        if (view != null && view.IsMine)
        {
            view.RPC("AddScoreRPC", RpcTarget.AllBuffered); 
            
        }
        
    }

    [PunRPC]
    private void AddScoreRPC()
    {
        PlayerStats.score++;
        scoreDisplay.text = PlayerStats.score.ToString();

        if (PlayerStats.score == 15)
        {
            
                PhotonNetwork.LoadLevel("GameLevel2");

        }
        else if (PlayerStats.score == 30)
        {
              gameOver.SetActive(true);
            
        }
    }



}
