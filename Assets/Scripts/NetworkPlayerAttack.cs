using UnityEngine;
using Unity.Netcode;

public class NetworkPlayerAttack : NetworkBehaviour
{
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private int damageAmount = 25;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private KeyCode attackKey = KeyCode.Space;

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (Input.GetKeyDown(attackKey))
        {
            RequestAttackServerRpc();
        }
    }

    [ServerRpc]
    private void RequestAttackServerRpc()
    {
        Vector3 attackCenter = transform.position + transform.forward;
        Collider[] hits = Physics.OverlapSphere(attackCenter, attackRange, playerLayer);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject)
            {
                continue;
            }
            NetworkPlayerHealth targetHealth = hit.GetComponent<NetworkPlayerHealth>();

            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damageAmount);
                break;

            }
        }

    }

}