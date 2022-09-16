using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{

    [SerializeField] TMP_InputField nicknameInput;

    void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            nicknameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else 
        {
            nicknameInput.text = "Player " + Random.Range(0, 1000).ToString("000");
            OnNicknameInputValueChange();
        }
    }
    public void OnNicknameInputValueChange()
    {
        PhotonNetwork.NickName = nicknameInput.text;
        PlayerPrefs.SetString("username", nicknameInput.text);
    }
}
