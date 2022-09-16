using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    private float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        LookUpDown();
        Move();
        Jump();
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
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pencet Spasi Doang");
            rb.AddForce(transform.up * jumpForce);
        }
*/
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Debug.Log("Pencet Spasi Doang DAN GROUNDED");
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public void setGroundedState(bool _grounded) 
    {
        grounded = _grounded;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);    
    }

}