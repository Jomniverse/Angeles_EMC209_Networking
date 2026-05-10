using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerNetworkController : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float groundedGravity = -2f;

    private CharacterController characterController;

    private float verticalVelocity;

    // 
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

        Vector2 inputDirection = new Vector2(horizontalInput, verticalInput);
        if (IsServer)
        {
            MovePlayer(inputDirection);
        }
        else
        {
            MovePlayerRpc(inputDirection);
        }

        
    }

    [Rpc(SendTo.Server)] // MARKS THE NEXT METHOD AS AN RPC THAT RUNS ON THE SERVER
    private void MovePlayerRpc(Vector2 movementInput)
    {
        MovePlayer(movementInput);
    }

    private void MovePlayer(Vector2 movementInput)
    {
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = groundedGravity;
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
