using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class AppManager : MonoBehaviourPunCallbacks
{
    public Animator loginScreenFade;
    public TMP_Text statusText;
    private string LocalPlayFabID;

    public string pUsername;

    private void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            pUsername = PlayerPrefs.GetString("username");
            Login();
        }
    }

    // Start is called before the first frame update
    public void CallLogin()
    {
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
        Debug.Log("Logging In/Creating Account..");
        statusText.text = "Logging In/Creating Account..";
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create!");
        statusText.text = "Successful login/account create!";

        PlayFabPlayerLoggedIn();

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = pUsername
        }, delegate (UpdateUserTitleDisplayNameResult result)
        {
            Debug.Log("Display Name Changed!");
        }, delegate (PlayFabError error)
        {
            Debug.Log("Error");
            Debug.Log(error.ErrorDetails);
        });

        LocalPlayFabID = result.PlayFabId;

    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account!");
        statusText.text = "Error while logging in/creating account!";
        if (error.Error == PlayFabErrorCode.AccountBanned)
        {
            Debug.Log("PLAYER IS BANNED");
        }
        Debug.Log(error.GenerateErrorReport());
    }

    public virtual void PlayFabPlayerLoggedIn()
    {
        loginScreenFade.Play("LoginScreenFadeOut");

        Invoke("setactive", 2);

        PhotonNetwork.ConnectUsingSettings();
    }

    void setactive()
    {
        loginScreenFade.gameObject.SetActive(false);
    }

    //public PhotonView playerPrefab;

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room.");
        //PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinRandomOrCreateRoom();
        PhotonNetwork.NickName = pUsername;
        Debug.Log("Username: " + PhotonNetwork.NickName);
    }
}
