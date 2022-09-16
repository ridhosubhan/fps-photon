using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();    
    }
    // Start is called before the first frame update
    void Start()
    {
        if (pv.IsMine) 
        {
            CreateController();
        }
    }

    void CreateController() 
    {
        Debug.Log("Player Control Instantiated");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
    }
}
