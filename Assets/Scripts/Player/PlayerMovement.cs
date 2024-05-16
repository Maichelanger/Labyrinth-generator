using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform player;
    private PlayerInputs playerInputs;
    private bool onDestination = true;
    private bool movingForward = true;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        player = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        playerInputs.Enable();

        playerInputs.Player.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        playerInputs.Player.Movement.performed -= ctx => Move(ctx.ReadValue<Vector2>());

        playerInputs.Disable();
    }

    private void FixedUpdate()
    {
        if (!onDestination)
        {
            MovePlayer();
        }
    }

    private void Move(Vector2 direction)
    {
        if (direction.y > 0)
        {
            movingForward = true;
            StartMoving();
        }
        else if (direction.y < 0)
        {
            movingForward = false;
            StartMoving();
        }
        else if (direction.x > 0)
        {
            player.Rotate(0, 90, 0);
        }
        else if (direction.x < 0)
        {
            player.Rotate(0, -90, 0);
        }
    }

    private void StartMoving()
    {
        StartCoroutine(OnDestinationDelay());
        MovePlayer();
    }

    // The player will move forward until it reaches the same xz position as a transform with the tag "Room Center"
    // We'll also have to check if the player collides with a wall to make it go backwards (So they return to the previous center). All walls will have the tag "Wall"
    private void MovePlayer()
    {
        if (movingForward)
        {
            player.position += player.forward * 0.1f;
        }
        else
        {
            player.position -= player.forward * 0.1f;
        }
    }

    IEnumerator OnDestinationDelay()
    {
        yield return new WaitForSeconds(0.3f);
        onDestination = false;
    }
}
