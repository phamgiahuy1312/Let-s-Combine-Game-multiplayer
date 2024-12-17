using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviour
{
    public int health = 2;
    public Text healthDisplay;
    public PlayerController playerController; // Tham chiếu tới PlayerController
    public Monster monster; // Tham chiếu tới Monster




    PhotonView view;
    public GameObject gameOver;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        healthDisplay.text = PlayerStats.health.ToString();
      
    }
    public void TakeDamage()
    {
        view.RPC("TakeDamageRPC", RpcTarget.All );
    }
    [PunRPC]
    void TakeDamageRPC()
    {
        PlayerStats.health--;
        if (PlayerStats.health <= 0)
        {
            Time.timeScale = 0;
            PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 0.01f;
            gameOver.SetActive(true);
            
        }
        healthDisplay.text = PlayerStats.health.ToString();

    }
}
