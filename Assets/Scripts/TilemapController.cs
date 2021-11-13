using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapController : MonoBehaviour
{
    public Transform spawnPoint;

    private void OnBecameInvisible()
    {
        transform.position = spawnPoint.position;
    }
}
