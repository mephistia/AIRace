using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void Update()
    {
        if (gameController.playing)
        {
            Vector3 newPosition = transform.position + Vector3.up * Time.deltaTime * gameController.velocity;
            transform.position = newPosition;
        }
    }

}
