using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class HUDController
{
    public static GameObject dialogueBox, travelBox, interactBox, shopGO, comboPanel, menuPanel, comboHUD, cursorImageGO;

    public static HUDValue goldHUD, foodHUD;
    public static HUDValueHealth healthHUD;
    public static ShopController shopController;
    public static ComboController comboController;
    public static MenuController menuController;
    public static ComboHUD comboHUDController;
    public static TravelUI travelController;
    public static Image cursorImage;
    public static Text dialogueText, goldAmtText, goldAddText;

    public static void Setup() {
        //Needs to be called initially from possibly player
        //This should all be in entire UI controller. The ui controller holds these and does the work. no finding
        //This class should just pass things on
        dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");
        dialogueText = GameObject.FindGameObjectWithTag("DialogueText").GetComponent<Text>();
        dialogueBox.SetActive(false);

        travelBox = GameObject.FindGameObjectWithTag("TravelBox");
        travelController = travelBox.GetComponent<TravelUI>();
        travelBox.SetActive(false);

        interactBox = GameObject.FindGameObjectWithTag("InteractBox");
        interactBox.SetActive(false);

        goldHUD = GameObject.FindGameObjectWithTag("GoldUI").GetComponent<HUDValue>();
        foodHUD = GameObject.FindGameObjectWithTag("FoodUI").GetComponent<HUDValue>();
        Debug.Log(foodHUD);
        healthHUD = GameObject.FindGameObjectWithTag("HealthUI").GetComponent<HUDValueHealth>();

        shopGO = GameObject.FindGameObjectWithTag("ShopUI");
        shopController = shopGO.GetComponent<ShopController>();
        shopGO.SetActive(false);

        comboPanel = GameObject.FindGameObjectWithTag("ComboUI");
        comboController = comboPanel.GetComponent<ComboController>();
        comboPanel.SetActive(false);

        menuPanel = GameObject.FindGameObjectWithTag("MenuUI");
        menuController = menuPanel.GetComponent<MenuController>();
        menuPanel.SetActive(false);

        comboHUD = GameObject.FindGameObjectWithTag("ComboHUD");
        comboHUDController = comboHUD.GetComponent<ComboHUD>();
        comboPanel.SetActive(false);

        cursorImageGO = GameObject.FindGameObjectWithTag("CursorImage");
        cursorImage = cursorImageGO.GetComponent<Image>();
        cursorImageGO.SetActive(false);

    }

    public static void SetText(string newText)
    {
        if (newText != null)
        {
            dialogueBox.SetActive(true);
            interactBox.SetActive(false);
            dialogueText.text = newText;
        }
        else
        {
            interactBox.SetActive(false);
            dialogueBox.SetActive(false);
        }
    }

    public static void ShowInteract()
    {
        interactBox.SetActive(true);
    }

    public static void HideInteract()
    {
        interactBox.SetActive(true);
    }

    public static void AddGold(int amt, int total)
    {
        goldHUD.AddValue(amt);
    }

    public static void AddFood(int amt, int total)
    {
        foodHUD.AddValue(amt);
    }

    public static void ChangeHealth(int amt, int total)
    {
        healthHUD.ChangeHealth(amt, total);
    }

    public static void ShowShop(bool on, Item[] items = null)
    {
        shopGO.SetActive(on);
        if (on)
        {
            shopController.SetItems(items);
        }
    }

    public static void ShowCombo(bool on, List<Attack> attacks = null, List<Attack> invAttacks = null)
    {
        menuPanel.SetActive(!on);
        comboPanel.SetActive(on);
        if (on)
        {
            comboController.SetAttacks(attacks);
            comboController.ShowAttackList(invAttacks);
        }
    }

    public static void ShowMenu(bool on)
    {
        comboPanel.SetActive(false);
        menuPanel.SetActive(on);
    }

    public static void SetMouseImage(bool on, Sprite img = null)
    {
        cursorImageGO.SetActive(on);
        if (on) { cursorImage.sprite = img; }
    }

    public static void ShowTravel(bool on, string travelName = null, int travelCost = 0, bool isLocked = false)
    {
        travelBox.SetActive(on);
        if (on)
        {
            travelController.ShowTravel(travelCost, travelName, isLocked);
        }
    }

    public static void SetComboHUD(int nextAttack)
    {
        comboHUDController.SetIcon(nextAttack);
    }
}
