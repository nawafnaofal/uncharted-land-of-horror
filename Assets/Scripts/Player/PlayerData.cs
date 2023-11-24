using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float currHealth;
    public float maxHealth;
    public float strength;
    public float speed;
    public int money;
    public int commonKeys;
    public int uncommonKeys;
    public int bossKeys;
    public int currentAmmo;
    public int maxAmmo;
    public float[] position;
    public string sceneName; // Menyimpan nama scene

    public PlayerData(Player player)
    {
        currHealth = player.currHealth;
        maxHealth = player.maxHealth;
        strength = player.strength;

        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            speed = playerMovement.Speed;
        }

        money = player.GetComponent<Inventory>().money;
        commonKeys = player.GetComponent<Inventory>().commonKeys;
        uncommonKeys = player.GetComponent<Inventory>().uncommonKeys;
        bossKeys = player.GetComponent<Inventory>().bossKeys;
        currentAmmo = player.GetComponent<Inventory>().currentAmmo;
        maxAmmo = player.GetComponent<Inventory>().maxAmmo;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        // Menyimpan nama scene
        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }
}
