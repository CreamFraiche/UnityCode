using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalk : MonoBehaviour
{

    private int curPhrase = 0;
    public string[] phrases;
    public bool hasEvent;

    private bool inConvo = false;

    void Start()
    {
        if(phrases == null)
        {
            //Turn off if no phrases
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inConvo)
        {
            SayPhrase(curPhrase);
        }
    }

    private void SayPhrase(int cur)
    {
        if(phrases.Length > cur)
        {
            HUDController.SetText(phrases[cur]);
            curPhrase++;
        }
        else
        {
            EndConvo();
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HUDController.ShowInteract();
            inConvo = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EndConvo();
        }
    }

    private void EndConvo()
    {
        HUDController.SetText(null);
        curPhrase = 0;
        inConvo = false;
        if (hasEvent)
        {
            foreach (QuestEvent QE in gameObject.GetComponents<QuestEvent>())
            {
                bool QuestDone = QE.TryEvent();
                if (!QuestDone) { break; }
            }
        }
    }
}

