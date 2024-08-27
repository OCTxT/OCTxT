using Photon.Pun;
using TMPro;
using UnityEngine;

public class PickAName : MonoBehaviour
{
    public TMP_InputField inputField;

    public TMP_Text status;

    public void changeNick()
    {
        PhotonNetwork.NickName = inputField.text;
        PlayerPrefs.SetString("username", inputField.text);

        status.text = "An app restart is required to set username.";
    }
}
