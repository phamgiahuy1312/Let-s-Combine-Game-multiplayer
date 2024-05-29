using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject monster;
    public float startTimeBtwSpawns;
    float timeBtwSpawns;
    public bool isGameStarted = false;


    private void Start()
    {
        timeBtwSpawns = startTimeBtwSpawns;
    }
    private void Update()
    {
        if(!isGameStarted || PhotonNetwork.IsMasterClient == false || PhotonNetwork.CurrentRoom.PlayerCount != 2 )
        {
            return;
        }

        if (timeBtwSpawns <= 0)
        {
            Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            PhotonNetwork.Instantiate(monster.name, spawnPosition, Quaternion.identity);
            timeBtwSpawns = startTimeBtwSpawns;
        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }

    // RPC để bắt đầu game
    [PunRPC]
    public void StartGame()
    {
        isGameStarted = true;
    }




}
