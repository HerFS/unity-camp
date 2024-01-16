using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal : MonoBehaviour
{
    Transform TeleportLocation;
    Transform PlayerLocation;
    PlayerController playerControl;
    bool useInteraction = false;

    void Start()
    {
        TeleportLocation = transform.GetChild(0);
    }

    void OnTriggerEnter(Collider other)
    {
        playerControl = other.GetComponent<PlayerController>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && playerControl.IsInteraction)
        {
            PlayerLocation = other.GetComponent<Transform>();
            useInteraction = true;
        }
    }

    void FixedUpdate()
    {
        if (useInteraction)
        {
            PlayerLocation.position = TeleportLocation.position;
            useInteraction = false;
        }
    }
}
