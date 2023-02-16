using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallTrigger : MonoBehaviour
{
    public float deathHeight;
    public string levelTarget;

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.y < deathHeight)
        {
            SceneManager.LoadScene(levelTarget);
        }
    }
}
