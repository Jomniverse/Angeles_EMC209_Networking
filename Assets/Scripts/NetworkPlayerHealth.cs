using UnityEngine;
using Unity.Netcode;
public class NetworkPlayerHealth : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    // THIS LINE CREATES NETWORK-SYNCED HEALTH VARAIABLE
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(
        100,
        NetworkVariableReadPermission.Everyone, // SHARES THE VALUE TO THE CLIENT AND HOST, HOST, CLIENT AND OTHER SERVER
        NetworkVariableWritePermission.Server // ONLY THE SERVER CAN CHANGE THE HEALTH VALUE 
        );

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            CurrentHealth.Value = maxHealth;
        }

        CurrentHealth.OnValueChanged += OnHealthCanged;
    }

    public override void OnNetworkDespawn()
    {
        CurrentHealth.OnValueChanged -= OnHealthCanged;
    }

    private void OnHealthCanged(int previousValue, int newValue)
    {
        Debug.Log($"{gameObject.name} health changed: {previousValue} -> {newValue}");
    }

    public void TakeDamage(int damageAmount)
    {
        if (!IsServer)
        {
            return;
        }

        CurrentHealth.Value -= damageAmount;
        CurrentHealth.Value = Mathf.Clamp(CurrentHealth.Value, 0, maxHealth);

        if (CurrentHealth.Value <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        CurrentHealth.Value = maxHealth;
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
        int randomIndex = Random.Range(0, spawns.Length);
        Transform selectedSpawn = spawns[randomIndex].transform;
        CharacterController controller = GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
        }

        transform.position = selectedSpawn.position;
        transform.rotation = selectedSpawn.rotation;

        if (controller != null)
        {
            controller.enabled = true;
        }

    }

}