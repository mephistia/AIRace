using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool playing = false;
    public Button playButton;
    public GameObject panelStart;
    public GameObject panelResults;
    public GameObject obstacle;
    public GameObject gameUI;
    public Transform spawnPoint;
    public Text scoreUI;
    public Text scoreResults;
    public float score = 0;
    float obstaclesSpawnSeconds = 3f;
    float timer = 0f;
    public float initialVelocity = 3f;
    public float velocity;


    void Start()
    {
        Time.timeScale = .0f;
        panelResults.SetActive(false);
        gameUI.SetActive(false);
        velocity = initialVelocity;
    }

    private void Update()
    {
        if (playing)
        {
            timer += Time.deltaTime;

            if (timer >= obstaclesSpawnSeconds)
            {
                timer = 0f;
                DecideSpawnObstacle();
            }

            score += (Time.deltaTime / 2f) * 10;
            scoreUI.text = score.ToString("N0");

            velocity += Time.deltaTime / 10f;
            Debug.Log(velocity);
        }
    }

    void DecideSpawnObstacle()
    {
        float r = Random.Range(0f, 1f);
        if (r >= .4f) // 60% de chance
        {
            GameObject.Instantiate(obstacle, spawnPoint.position, Quaternion.identity, transform.parent);
        }
    }

    public void StartGame()
    {
        playing = true;
        Time.timeScale = 1f;
        panelStart.SetActive(false);
        panelResults.SetActive(false);
        gameUI.SetActive(true);
    }

    public void ResetGame()
    {
        GameObject[] obstacles = FindObstacles();

        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }

        scoreResults.text = score.ToString("N0");

        score = 0;
        velocity = initialVelocity;

        // pausar e esperar interação do jogador para reiniciar:
        //PauseGameOver();
    }

    public GameObject[] FindObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        return obstacles;
    }

    public void PauseGameOver()
    {
        playing = false;
        Time.timeScale = 0f;

        gameUI.SetActive(false);
        panelStart.SetActive(true);
        panelResults.SetActive(true);
    }



}
