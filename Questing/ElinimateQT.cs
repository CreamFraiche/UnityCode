using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElinimateQT : QuestTrigger
{
    public GameObject[] enemies;
    public int CompletionCheckRate = 60, frame;
    public override void Update()
    {
        frame++;
        if(frame >= CompletionCheckRate)
        {
            frame = 0;
            foreach (GameObject enemy in enemies)
            {
                if (!enemy.GetComponent<NPC>().isDead)
                {
                    return;
                }
            }
            CompleteQuest();
            enabled = false;
        }
    }
}
