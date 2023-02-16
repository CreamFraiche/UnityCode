using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Item[] saleItems;
    public ShopController shopUIController;
    public PlayerInteractions playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteractions>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            ShopHolder.SetShop(this);
            HUDController.ShowShop(true, saleItems);
            StartShopping();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HUDController.SetText(null);
            HUDController.ShowShop(false);
            EndShopping();
        }
    }

    public void BuyItem(int itemIndex)
    {
        shopUIController = GameObject.FindGameObjectWithTag("ShopUI").GetComponent<ShopController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteractions>();

        Debug.Log(saleItems[itemIndex]);

        if(playerController.gold >= saleItems[itemIndex].value)
        {
            playerController.ChangeGold(saleItems[itemIndex].value * -1);
            playerController.ReceiveItem(saleItems[itemIndex]);
            shopUIController.RemoveItem(itemIndex);
        }
        else
        {
            HUDController.SetText("You cannot afford this");
        }

    }

    private void StartShopping()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void EndShopping()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
