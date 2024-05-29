using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public float loadingTime = 5f;
    
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }



    // Update is called once per frame
    public override void OnConnectedToMaster()
    {
        Invoke("LoadMainMenu", loadingTime);
        
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
