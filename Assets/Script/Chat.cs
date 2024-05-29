using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;
using Photon.Realtime;

public class Chat : MonoBehaviour
{
    public InputField inputField;
    public GameObject Message;
    public GameObject Content;
    public Text RoomID;

    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        // Thêm s? ki?n l?ng nghe vào các s? ki?n c?a input field
        inputField.onEndEdit.AddListener(OnEndEdit);
        EventTrigger trigger = inputField.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entrySelect = new EventTrigger.Entry();
        entrySelect.eventID = EventTriggerType.Select;
        entrySelect.callback.AddListener((eventData) => { OnSelect(); });
        trigger.triggers.Add(entrySelect);

        RoomID.text = "Chat Box Room ID: " + PhotonNetwork.CurrentRoom.Name;

    }

    private void OnDestroy()
    {
        // Remove listeners when the script is destroyed
        inputField.onEndEdit.RemoveListener(OnEndEdit);
        // Removing the EventTrigger component safely
        EventTrigger trigger = inputField.GetComponent<EventTrigger>();
        if (trigger != null)
        {
            Destroy(trigger);
        }
    }

    public void SendMessage()
    {
        GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All, (PhotonNetwork.NickName + " : " + inputField.text));
        inputField.text = "";
    }

    [PunRPC]
    public void GetMessage(string ReceiMessage)
    {
        GameObject M = Instantiate(Message, Vector3.zero, Quaternion.identity, Content.transform);
        M.GetComponent<Message>().MyMessage.text = ReceiMessage;
    }

    private void OnEndEdit(string text)
    {
        if (playerController != null)
        {
            playerController.IsTyping = false;
        }
    }

    private void OnSelect()
    {
        if (playerController != null)
        {
            playerController.IsTyping = true;
        }
    }
}
