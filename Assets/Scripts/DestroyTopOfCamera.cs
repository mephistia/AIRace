using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTopOfCamera : MonoBehaviour
{
    float maxYPos = 16f;
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= maxYPos)
        {
            Destroy(gameObject);
        }
    }
}
