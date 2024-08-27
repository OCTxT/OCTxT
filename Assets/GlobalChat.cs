using Photon.Pun;
using TMPro;
using UnityEngine;

public class GlobalChat : MonoBehaviour
{
    public AppManager AppManager;
    public TMP_InputField inputField;
    public GameObject Message;
    public GameObject Content;

    public void SendMessage()
    {
        // Get the local player's nickname or ID

        // Send the message and author name via RPC
        GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All, PhotonNetwork.NickName, inputField.text);

        inputField.text = "";
    }

    [PunRPC]
    public void GetMessage(string authorName, string receivemessage)
    {
        GameObject M = Instantiate(Message, Vector3.zero, Quaternion.identity, Content.transform);
        M.GetComponent<Message>().message.text = receivemessage;
        M.GetComponent<Message>().author.text = authorName;
    }
}
