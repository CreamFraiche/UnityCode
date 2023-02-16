using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Travel : MonoBehaviour
{

    public bool isOpen;
    public int foodCost;
    public string levelTarget;

    private GameObject player;

    private bool opening = false;
    public string lockName;

    public void Start()
    {
        Debug.Log("Checking: " + lockName + " it is: " + PlayerPrefs.GetInt(lockName));
        if (PlayerPrefs.GetInt(lockName) != 0)
        {
            UnlockTravel();
        }
    }

    public void UnlockTravel()
    {
        PlayerPrefs.SetInt(lockName, 1);
        isOpen = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Colliding");
            player = other.gameObject;
            HUDController.ShowTravel(true, levelTarget, foodCost, !isOpen);
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
            if (isOpen)
            {
                Debug.Log("Colliding and F");
                PlayerInteractions playerScript = player.GetComponent<PlayerInteractions>();
                HUDController.HideInteract();
                if (foodCost <= playerScript.food)
                {
                    playerScript.food -= foodCost;
                    playerScript.SaveValues();
                    //Set Inventory Holder
                    InventoryHolder.Instance.comboList = playerScript.playerController.comboList;
                    InventoryHolder.Instance.attackInv = playerScript.playerController.attackInv;

                    //Save to Player Prefs
                    InventoryHolder.Instance.Save(playerScript.equipment);

                    SceneManager.LoadScene(levelTarget);
                }
                else
                {
                    HUDController.SetText("Insufficient Food: " + foodCost + " food is required");
                }
            }
            else
            {
                HUDController.SetText("This Gate is currently Closed, try talking to the towns people");
            }
        }
    }

    new public void OpenGate()
    {
        Debug.Log("Travel Open Gate");
        PlayerPrefs.SetInt(lockName, 1);
        isOpen = true;
    }
}
