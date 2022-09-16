using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    PlayerController PlayerController;

    void Awake()
    {
        PlayerController = GetComponentInParent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerController.gameObject)
            return;

        PlayerController.setGroundedState(true);    
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerController.gameObject)
            return;

        PlayerController.setGroundedState(false);
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("Name : " + other.gameObject.name);
        Debug.Log("Name : " + PlayerController.gameObject.name);

        if (other.gameObject == PlayerController.gameObject)
            return;

        PlayerController.setGroundedState(true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == PlayerController.gameObject)
            return;

        PlayerController.setGroundedState(true);
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == PlayerController.gameObject)
            return;

        PlayerController.setGroundedState(false);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == PlayerController.gameObject)
            return;

        PlayerController.setGroundedState(true);
    }
}
