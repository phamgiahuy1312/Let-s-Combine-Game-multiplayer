using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
//using UnityEditor.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float speed;
    float resetSpeed;
    public float dashSpeed;
    public float dashTime;
    public Animator animator;
    public float runSpeed = 4f;
    bool isFacingRight = true;
    public bool IsTyping = false;



    public GameObject PlayerDeathFx;
    PhotonView view;
    Health healthScript;
    LineRenderer rend;

    public float minX, maxX, minY, maxY;

    public TextMeshPro nameDisplay;
    private void Start()
    {
        Time.timeScale = 1;
        resetSpeed = speed;
        view = GetComponent<PhotonView>();
        healthScript = FindObjectOfType<Health>();
        rend = FindObjectOfType<LineRenderer>();

        if (view.IsMine)
        {
            nameDisplay.text = PhotonNetwork.NickName;
        } else
        {
            nameDisplay.text = view.Owner.NickName;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //independent control
        if (view.IsMine)
        {

            if (!IsTyping)
            {
                Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal") * runSpeed, Input.GetAxisRaw("Vertical") * runSpeed);
                float moveMagnitude = moveInput.magnitude;

                animator.SetFloat("speed", moveMagnitude);
                view.RPC("SyncAnimation", RpcTarget.All, moveMagnitude);

                Vector2 moveAmount = moveInput.normalized * speed * Time.deltaTime;
                transform.position += (Vector3)moveAmount;

                Wrap();

                if (Input.GetKeyDown(KeyCode.Space) && moveInput != Vector2.zero)
                {
                    StartCoroutine(Dash());
                }

                if (isFacingRight && Input.GetAxisRaw("Horizontal") < 0 || !isFacingRight && Input.GetAxisRaw("Horizontal") > 0)
                {
                    Flip();
                }

                rend.SetPosition(0, transform.position);
            }
        } 
        else
        {
            rend.SetPosition(1, transform.position);
        }

        


    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

        nameDisplay.rectTransform.localScale = new Vector3(nameDisplay.rectTransform.localScale.x * -1, nameDisplay.rectTransform.localScale.y, nameDisplay.rectTransform.localScale.z);
    }

    IEnumerator Dash()
    {
        //set speed to dash speed
        speed = dashSpeed;
        //wait for dash time
        yield return new WaitForSeconds(dashTime);
        speed = resetSpeed;
    }

    void Wrap()
    {
        //wrap player
        if(transform.position.x < minX)
        {
            transform.position = new Vector2(maxX, transform.position.y);
        }
        if (transform.position.x > maxX)
        {
            transform.position = new Vector2(minX, transform.position.y);
        }
        if (transform.position.y < minY)
        {
            transform.position = new Vector2(transform.position.x, maxY);
        }
        if (transform.position.y > maxY)
        {
            transform.position = new Vector2(transform.position.x, minY);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //kiểm tra xem có phải là player của mình không
        if(view.IsMine)
        {
            if (collision.CompareTag("Monster"))
            {
                healthScript.TakeDamage();

                view.RPC("showDeathFX", RpcTarget.All); 
            }
        }
    }

    [PunRPC]
    private void showDeathFX()
    {
        Instantiate(PlayerDeathFx, transform.position, Quaternion.identity);
    }
    [PunRPC]
    private void SyncAnimation(float speed)
    {
        animator.SetFloat("speed", speed);
    }
}
