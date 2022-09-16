using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    [SerializeField] Item[] items;
    int itemIndex;
    int previousItemIndex = -1;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    PhotonView pv;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    PlayerManager playerManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
        if (pv.IsMine)
        {
            EquipItem(0);
        }
        else 
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    }

    void Update()
    {
        if (!pv.IsMine)
            return;

        LookUpDown();
        Move();
        Jump();

        for (int i = 0; i < items.Length; i++)
        { 
            if(Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        /*switch item on scroll mouse*/
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (itemIndex >= items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(itemIndex + 1);
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f) 
        {
            if (itemIndex <= 0)
            {
                EquipItem(items.Length-1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }
        }

        if (Input.GetMouseButton(0))
        {
            items[itemIndex].Use();
        }

        if (transform.position.y < -10f) //Mati kallo jatuh dari ketinggian
        {
            Die();
        }
    }

    void LookUpDown() 
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);

    }

    void Jump() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Debug.Log("Pencet Spasi Doang DAN GROUNDED");
            rb.AddForce(transform.up * jumpForce);
        }
    }

    void EquipItem(int _index) 
    {
        if (_index == previousItemIndex)
            return;
        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if (pv.IsMine)
        { 
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!pv.IsMine && targetPlayer == pv.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    public void setGroundedState(bool _grounded) 
    {
        grounded = _grounded;
    }

    void FixedUpdate()
    {
        if (!pv.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);    
    }

    public void TakeDamage(float damage)
    {
        pv.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!pv.IsMine)
            return;

        Debug.Log("Took : " + damage + " damage");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die(); 
        }
        
    }

    void Die() 
    {
        playerManager.Die();  
    }
}
