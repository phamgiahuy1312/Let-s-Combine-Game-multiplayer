
using UnityEngine;
using Photon.Pun;

public class ChangeColors : MonoBehaviourPunCallbacks
{
    private SpriteRenderer rend;
    private Color chosenColor;

    // Tạo một mảng các màu sắc cụ thể cho mỗi người chơi
    public Color[] playerColors = new Color[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow
    };

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        if (photonView.IsMine)
        {
            // Chọn màu sắc cho nhân vật của người chơi hiện tại
            ChooseColor(); 
            // Gửi màu sắc đã chọn cho các người chơi khác
            photonView.RPC("SyncColor", RpcTarget.OthersBuffered, chosenColor.r, chosenColor.g, chosenColor.b);
        }
    }

    private void ChooseColor()
    {
        // Chọn một màu sắc ngẫu nhiên từ mảng playerColors
        chosenColor = playerColors[Random.Range(0, playerColors.Length)];
        // Áp dụng màu sắc đã chọn cho nhân vật
        rend.color = chosenColor;
    }

    [PunRPC]
    private void SyncColor(float r, float g, float b)
    {
        if (rend == null)
        {
            rend = GetComponent<SpriteRenderer>();
        }

        // Nhận màu sắc từ máy chủ và cập nhật màu sắc cho nhân vật của người chơi
        rend.color = new Color(r, g, b);
    }
}
