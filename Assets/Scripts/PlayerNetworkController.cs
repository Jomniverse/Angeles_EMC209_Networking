using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkController : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    [SerializeField] float gravity = -9.81f;
    [SerializeField] float groundedGravity = -2f;

    [SerializeField] float jumpForce = 5f;

    private CharacterController characterController;

    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 inputDirection =
            new Vector2(horizontalInput, verticalInput);

        bool jumpPressed = Input.GetButtonDown("Jump");

        if (IsServer)
        {
            MovePlayer(inputDirection, jumpPressed);
        }
        else
        {
            MovePlayerRpc(inputDirection, jumpPressed);
        }
    }

    [Rpc(SendTo.Server)]
    private void MovePlayerRpc(
        Vector2 movementInput,
        bool jumpPressed
    )
    {
        MovePlayer(movementInput, jumpPressed);
    }

    private void MovePlayer(
        Vector2 movementInput,
        bool jumpPressed
    )
    {
        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0)
            {
                verticalVelocity = groundedGravity;
            }

            
            if (jumpPressed)
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        Vector3 horizontalMovement = moveDirection * moveSpeed;
        Vector3 verticalMovement = Vector3.up * verticalVelocity;
        Vector3 finalMovement = horizontalMovement + verticalMovement;
        characterController.Move(finalMovement * Time.deltaTime);
    }
}