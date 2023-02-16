using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectQE : QuestEvent
{
    public GameObject rewardObject;

    public override void DoEvent()
    {
        Debug.Log("Doing Object Event");
        PlayerPrefs.SetInt(QuestID, 2);
        rewardObject.SetActive(true);
    }
}
