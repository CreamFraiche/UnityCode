using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private bool opening = false;
    public string lockName;

    public void Start()
    {
        Debug.Log("Checking: " + lockName + " it is: " + PlayerPrefs.GetInt(lockName));
        if (PlayerPrefs.GetInt(lockName) != 0)
        {
            OpenGate();
        }
    }

    public void OpenGate()
    {
        Debug.Log("Regular Gate Open");
        opening = true;
        StartCoroutine(Done());
    }

    void Update()
    {
        if (opening)
        {
            Vector3 newPos = new Vector3(transform.position.x - 5f, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPos, 5);
        }
    }

    IEnumerator Done()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
