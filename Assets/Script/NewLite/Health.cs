using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    private int maxHealth;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = 20;

    public RectTransform healthbar;
    private float sizeHealthBar;
    public bool destrotOnDeath;
    private NetworkStartPosition[] spawnPoints;
    
    void Start()
    {
        maxHealth = currentHealth; // из за сетевой фигни сохраняем макс ХП
        sizeHealthBar = healthbar.sizeDelta.x; //запоминаем размер полосы здоровья

        if (isLocalPlayer)
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        TakeDamage(0);
        OnChangeHealth(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (destrotOnDeath)
                Destroy(gameObject);
            else
            {
                currentHealth = maxHealth;
                RpcRespawn();
            }
        }
        OnChangeHealth(currentHealth); //показываем текущий уровень ХП
        Debug.Log("TakeDamag: " + currentHealth + "/" + maxHealth);
    }

    void OnChangeHealth(int health)
    {
        healthbar.sizeDelta = new Vector2((sizeHealthBar / maxHealth) * health, healthbar.sizeDelta.y);
    }
    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            Vector3 spawnPoint = Vector3.zero;

            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }
            transform.position = spawnPoint;
            Debug.Log("haha Dead");
        }
    }
}
