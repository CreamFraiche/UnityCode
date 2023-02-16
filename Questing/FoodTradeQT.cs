using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTradeQT : QuestTrigger
{
    public int foodRequired;
    // Start is called before the first frame update

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerInteractions playerScript = other.gameObject.GetComponent<PlayerInteractions>();

            Debug.Log("Food Quest Checking Complete");
            if (playerScript.food > foodRequired)
            {
                playerScript.CollectFood(-foodRequired);
                CompleteQuest();
            }
            else
            {
                //HUDController.SetText("Insufficent food for quest");
            }
        }
    }
}
