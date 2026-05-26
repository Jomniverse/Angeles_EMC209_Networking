using UnityEngine;
using Unity.Netcode;

public class NetworkPlayerHealth : NetworkBehaviour
{
    public GameObject healthTextPrefab;
    public GameObject damageTextPrefab;

    [SerializeField] private int maxHealth = 100;

    private TextMesh healthText;

    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(
        100,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            CurrentHealth.Value = maxHealth;
        }

        CurrentHealth.OnValueChanged += OnHealthChanged;

        ShowHealth();
    }

    public override void OnNetworkDespawn()
    {
        CurrentHealth.OnValueChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int previousValue, int newValue)
    {
        Debug.Log($"{gameObject.name} health changed: {previousValue} -> {newValue}");

        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        if (!IsOwner)
        {
            return;
        }

        if (healthText != null)
        {
            healthText.text = "HP: “ + CurrentHealth.Value “ / “ + maxHealth.ToString";
        }
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

        if (damageTextPrefab)
        {
            ShowDamageTextClientRpc(damageAmount);
        }
    }

    [ClientRpc]
    private void ShowDamageTextClientRpc(int damageAmountText)
    {
        var damage = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, transform);

        damage.GetComponent<TextMesh>().text = damageAmountText.ToString();
    }

    private void ShowHealth()
    {
        if (!IsOwner)
        {
            return;
        }

        var health = Instantiate (healthTextPrefab, transform.position, Quaternion.identity, transform);

        healthText = health.GetComponent<TextMesh>();

        UpdateHealthText();
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