using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvent : MonoBehaviour
{
    //This script is for quest completion (rewards)

    //0 not done
    //1 done not collected
    //2 done and collected

    public string QuestID;

    public void Start()
    {
        int QuestStatus = PlayerPrefs.GetInt(QuestID);
        if(QuestStatus == 2)
        {
            this.enabled = false;
        }
    }

    public bool TryEvent() {
        int QuestStatus = PlayerPrefs.GetInt(QuestID);
        Debug.Log("Try event on: " + QuestID + "is: " + QuestStatus);

        if (QuestStatus >= 1)
        {
            if(QuestStatus == 1)
            {
                DoEvent();
            }
            return true;
        }
        else
        {
            Debug.Log("Quest Not Complete: Quest Pref is " + QuestStatus);
            return false;
        }
    }

    public virtual void DoEvent()
    {
        Debug.Log("Quest Event Do Event");
    }
}
