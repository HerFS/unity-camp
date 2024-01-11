using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal : MonoBehaviour
{
    Transform TeleportLocation;
    [SerializeField]
    Transform PlayerLocation;
    InputTest inputTest;
    bool isKeyDown;
    bool isTrue = false;

    void Start()
    {
        TeleportLocation = transform.GetChild(0);
    }

    void OnTriggerStay(Collider other)
    {
        inputTest = other.GetComponent<InputTest>();
        if (other.tag == "Player" && inputTest.isInteraction)
        {
            PlayerLocation = other.GetComponent<Transform>();
            isTrue = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //gameObject.GetComponent<SphereCollider>().enabled = false;

        switch (other.name) {
            case "Hello":
                print("Hello");
                break;
            case "Bye":
                print("Bye");
                break;
            default:
                print(other.tag);
                break;
        }
    }

    void FixedUpdate()
    {
        if (isTrue)
        {
            PlayerLocation.position = TeleportLocation.position;
            isTrue = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //isKeyDown = Input.GetKeyDown(KeyCode.Space);
    }
}
