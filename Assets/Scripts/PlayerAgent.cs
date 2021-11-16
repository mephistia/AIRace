using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PlayerAgent : Agent
{
    CharacterController characterController;
    public GameController gameController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public override void OnEpisodeBegin()
    {
        characterController.EndDeath();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition); // posição atual
        sensor.AddObservation(CalculateNearestObstacleDistance()); // distância do obstáculo mais próximo
        sensor.AddObservation(gameController.velocity); // velocidade do jogo
        sensor.AddObservation(characterController.canJump); // pode pular?
        sensor.AddObservation(characterController.isAlive); // está vivo?
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Ações (2): mover na vertical ou pular
        float movement;
        bool shouldJump = false;
        movement = actions.ContinuousActions[0]; // movimentação no eixo vertical
        shouldJump = actions.ContinuousActions[1] > 0.5f; // deve pular quando decidir > 0.5

        // Aplicar ações:
        // movimentar para cima (> 0.6),
        // baixo (< 0.3),
        // ou parar (entre 0.3 e 0.6)
        if (movement > 0.6f)
        {
            characterController.MoveUp();
        }
        else if (movement < 0.3f)
        {
            characterController.MoveDown();
        }

        // ou pular
        if (shouldJump)
        {
            characterController.Jump();
        }

        // Recompensas:
        if (characterController.isAlive)
        {
            // score do frame (+1 por estar vivo, +10 quando pula obstáculo)
            SetReward(characterController.frameScore); 
        }
        else
        {
            // pune por morrer
            SetReward(-10000f);
            EndEpisode();
        }

    }

    public float CalculateNearestObstacleDistance()
    {
        GameObject[] obstacles = gameController.FindObstacles();
        float nearestDistance = Mathf.Infinity;

        // pra cada obstáculo
        foreach (GameObject o in obstacles)
        {
            Vector3 directionToTarget = o.transform.position - this.transform.position;
            float sqrDistance = directionToTarget.sqrMagnitude;

            // se for mais perto que o atual, e estiver embaixo do personagem
            if (sqrDistance < nearestDistance && o.transform.position.y < this.transform.position.y)
            {
                // seta como o mais perto
                nearestDistance = sqrDistance;
            }
        }

        return nearestDistance;
    }
}
