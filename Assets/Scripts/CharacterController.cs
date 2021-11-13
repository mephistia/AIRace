using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public GameController gameController;
    bool canMove = true;
    float characterVelocity = 4f;
    bool ignoreObstacle = false;
    bool canJump = true;
    Animator animator;
    Vector3 startPos;
    static float jumpCooldown = 0.5f;
    WaitForSeconds waitJumpCooldown = new WaitForSeconds(jumpCooldown);

    private void Start()
    {
        SetStartVariables();
        animator = GetComponent<Animator>();
    }

    void SetStartVariables()
    {
        startPos = transform.position;
        canMove = true;
        ignoreObstacle = false;
        canJump = true;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        // bloqueia movimento quando morre
        if (canMove)
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                MoveDown();
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                MoveUp();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                // tem cooldown de pulo
                if (canJump)
                {
                    Jump();
                }
            }

        }
    }

    public void MoveDown()
    {
        // não pode ultrapassar a tela
        if (transform.position.y >= -6f)
        {
            Vector3 newPosition = transform.position - Vector3.up * Time.deltaTime * characterVelocity;
            transform.position = newPosition;
        }
    }

    public void MoveUp()
    {
        // não pode ultrapassar a tela
        if (transform.position.y <= 7f)
        {
            Vector3 newPosition = transform.position + Vector3.up * Time.deltaTime * characterVelocity;
            transform.position = newPosition;
        }
    }

    public void Jump()
    {
        canJump = false;
        ignoreObstacle = true;
        animator.Play("SlimeJump");
    }

    // chamado no final da animação de pulo: 
    public void EndJump()
    {
        animator.Play("SlimeWalk");
        ignoreObstacle = false;

        StartCoroutine(JumpCooldown());
    }

    IEnumerator JumpCooldown()
    {
        yield return waitJumpCooldown;
        canJump = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ignoreObstacle)
        {
            canMove = false;
            animator.Play("SlimeDeath");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ignoreObstacle)
        {
            gameController.score += 10;
        }
    }

    // chamado no final da animação de morte
    public void EndDeath()
    {
        gameObject.transform.position = startPos;
        SetStartVariables();
        animator.Play("SlimeWalk"); 
        gameController.ResetGame();
    }

}
