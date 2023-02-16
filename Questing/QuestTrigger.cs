using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    //When called, completes the quest

    public string QuestID;
    public bool display;

    public void Start()
    {
        int questStatus = PlayerPrefs.GetInt(QuestID);
        if(questStatus > 0)
        {
            this.enabled = false;
        }
    }
    public void CompleteQuest() {
        Debug.Log("Calling Complete Quest: " + QuestID);
        if (PlayerPrefs.GetInt(QuestID) != 2) {
            PlayerPrefs.SetInt(QuestID, 1);
            Debug.Log("Quest Completed");
            if (display)
            {
                HUDController.SetText("Quest Completed -" + QuestID);
            }
        }
    }

    public virtual void Update()
    {
        //Check for quest completion
    }
}
