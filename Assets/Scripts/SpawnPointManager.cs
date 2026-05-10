using UnityEngine;
using Unity.Netcode;

// CREATES A NETWORK-AWARE SPAWN MANAGER SCRIPT
public class SpawnPointManager : NetworkBehaviour
{
    // STORES WHICH SPAWN POINT SHOULD BE USED NEXT.
    // STATIC MEANS ALL PLAYER OBJECTS SHARE THIS VALUE.
    private static int nextSpawnIndex;

    // RUNS WHEN THE PLAYER OBJECT IS SPAWNED BY NETCODE.
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }

        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");

        if (spawnPointObjects.Length == 0)
        {
            Debug.Log("No SpawnPoint Detected");
            return;
        }

        Transform selectedSpawnPoint = spawnPointObjects[nextSpawnIndex].transform;
        CharacterController characterController = GetComponent<CharacterController>();

        // TEMPORARILY DISABLES THE CHARACTER CONTROLLER
        // BEFORE TELEPORTING THE PLAYER.
        if (characterController != null)
        {
            characterController.enabled = false;
        }
        
        // MOVES THE PLAYER THE SELECTED SPAWN POINT
        transform.position = selectedSpawnPoint.position;

        // ROTATES THE PLAYER TO MATCH THE SPAWN POINT.
        transform.rotation = selectedSpawnPoint.rotation;

        if (characterController != null)
        {
            characterController.enabled = true;
        }

        nextSpawnIndex++;

        if (nextSpawnIndex >= spawnPointObjects.Length)
        {
            nextSpawnIndex = 0;
        }

        MultiplayerMenu.instance.playerCounts.Value++;
    }
}
