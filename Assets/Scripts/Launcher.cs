using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        MenuManager.instance.OpenMenu("title");
        Debug.Log("Joinned to Lobby");
    }

    public void CreateRoom() 
    {
        if (string.IsNullOrEmpty(roomNameInputField.text)) 
        {
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Failed Creating Room: " + message;
        MenuManager.instance.OpenMenu("error menu");
    }

    public void LeaveRoom() 
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("title");
    }

}
