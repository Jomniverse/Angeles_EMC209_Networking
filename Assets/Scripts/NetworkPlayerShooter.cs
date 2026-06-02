using UnityEngine;
using Unity.Netcode;
public class NetworkPlayerShooter : NetworkBehaviour
{
    [SerializeField] GameObject projectilePrefab; // PROJECTILE PREFAB
    [SerializeField] Transform firePoint; // WHERE THE PROJECTILE SPAWN
    [SerializeField] float fireCooldown = 0.1f; // FIRE RATE
    [SerializeField] KeyCode fireButton = KeyCode.Mouse0;

    private float nextFireTime;
    
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(fireButton) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            RequestShootServerRpc(firePoint.position, firePoint.forward);
        }
    }

    [ServerRpc]
    private void RequestShootServerRpc(Vector3 spawnPosition, Vector3 shootDirection)
    {
        // INSTANTIATE = CREATE OBJECT ON THE SERVER
        // SPAWN = TELLS UNITY NETWORK TO SHOW THIS OBJECT TO ALL CONNECTED PLAYER
        GameObject porjectileInstantiate = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.LookRotation(shootDirection)
            );

        NetworkObject networkObject = porjectileInstantiate.GetComponent<NetworkObject>();
        networkObject.Spawn();
    }
}
