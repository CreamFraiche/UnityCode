using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockTravelQE : QuestEvent
{
    public Travel travel;
    override public void DoEvent()
    {
        Debug.Log("Opening Travel Gate");
        travel.GetComponent<Travel>().UnlockTravel();
    }
}
