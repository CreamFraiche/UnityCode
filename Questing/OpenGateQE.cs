using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGateQE : QuestEvent
{
    public Gate gate;
    new public void DoEvent()
    {
        gate.GetComponent<Gate>().OpenGate();
    }
}
