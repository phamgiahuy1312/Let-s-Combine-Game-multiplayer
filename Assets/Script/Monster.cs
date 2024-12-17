using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Monster : MonoBehaviour
{
    public Animator animator;
    public float speed;
    public GameObject deathFX;

    private PhotonView view;
    private PlayerController[] players;
    private PlayerController nearestPlayer;
    private Score score;

    private bool isFrozen = false;
    private void Start()
    {
        view = GetComponent<PhotonView>();
        if (view == null)
        {
            Debug.LogError("Missing PhotonView component on Monster!");
            return;
        }

        players = FindObjectsOfType<PlayerController>();
        if (players.Length < 2)
        {
            Debug.LogError("Not enough PlayerController objects found!");
            return;
        }

        score = FindObjectOfType<Score>();
        if (score == null)
        {
            Debug.LogError("Score component not found!");
            return;
        }
    }

    private void Update()
    {
        if (isFrozen) return;

        if (players.Length < 2)
            return; // Không đủ người chơi, không cần tiếp tục

        float distanceOne = Vector2.Distance(transform.position, players[0].transform.position);
        float distanceTwo = Vector2.Distance(transform.position, players[1].transform.position);

        nearestPlayer = distanceOne < distanceTwo ? players[0] : players[1];

        if (nearestPlayer != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, nearestPlayer.transform.position, speed * Time.deltaTime);
            animator.SetFloat("speed", speed);
        }
    }

    // Set monster frozen state
    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(PhotonNetwork.IsConnected && view.IsMine && gameObject != null)
            {
                if (collision.CompareTag("Line"))
                {
                    score.AddScore();
                    view.RPC("SpawnParticle",RpcTarget.AllBuffered);
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }

    [PunRPC]
    void SpawnParticle()
    {
        // Tạo hiệu ứng particle(Deathfx) tại vị trí của monster
        Instantiate(deathFX, transform.position, Quaternion.identity);
    }
}
