using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{

    private Vector3 mousePosition;
    public float moveSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
