using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView pv;

    GameObject controller;

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

        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] {pv.ViewID});
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
}
