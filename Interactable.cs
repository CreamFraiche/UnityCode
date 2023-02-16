using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject player, activate;
    public bool turnOff;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Colliding");
            player = other.gameObject;
            HUDController.SetText("Press F to Interact");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HUDController.ShowTravel(false);
            HUDController.SetText(null);
            player = null;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && player != null)
        {
            Triggered();
        }
    }

    public void Triggered()
    {
        if (activate)
        {
            activate.SetActive(!turnOff);
            HUDController.SetText(null);
            this.enabled = false;
        }
        else
        {
            Debug.Log("Interact Triggered: Base Class (no action taken)");
        }
    }
}
