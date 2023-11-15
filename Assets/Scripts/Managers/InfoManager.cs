using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class InfoManager : MonoBehaviour
{
    static InfoManager instance;
    public static InfoManager Instance { get { return instance; } }
    public Vector2 NewPlayerPosition { get; set; }
    public float PlayerMaxHealth { get; set; }
    public float PlayerHealth { get; set; }
    public float PlayerMagic { get; set; }
    public int AmmoLeft { get; set; }
    public int CommonKeys { get; set; }
    public int UnCommonKeys { get; set; }
    public int BossKeys { get; set; }
    public int Money { get; set; }
    public List<Item> items;
    public Dictionary<string, bool> chests = new Dictionary<string, bool>();
    public Dictionary<string, bool> buttons = new Dictionary<string, bool>();
    public Dictionary<string, DoorType> doors = new Dictionary<string, DoorType>();

    // Define scenes that require the "Transition" object
    private string[] scenesWithTransition = { "MainMenu", "Main", "HouseInterior", "Cave", "CreditScene", };

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        SceneManager.sceneLoaded += InitiateLevel;
    }

    public void InitiateLevel(Scene scene, LoadSceneMode mode)
    {
        //GameObject.FindWithTag("Transition").GetComponent<UnityEngine.UI.Image>().enabled = true;

        Debug.Log("Initiating level for scene: " + scene.name);

        bool sceneRequiresTransition = Array.Exists(scenesWithTransition, s => s == scene.name);

        if (sceneRequiresTransition)
        {
            // Try to find the GameObject with the tag "Transition"
            GameObject transitionObject = GameObject.FindWithTag("Transition");

            if (transitionObject != null)
            {
                // Try to get the UnityEngine.UI.Image component
                UnityEngine.UI.Image transitionImage = transitionObject.GetComponent<UnityEngine.UI.Image>();

                if (transitionImage != null)
                {
                    // Enable the image component
                    transitionImage.enabled = true;
                }
                else
                {
                    Debug.LogError("Image component not found on the object with tag 'Transition'.");
                }
            }
            else
            {
                // Optionally log a message instead of an error
                Debug.LogWarning("Object with tag 'Transition' not found in scene: " + scene.name);
            }
        }

    }

    public void UpdateStats()
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Inventory playerInventory = player.GetComponent<Inventory>();
        PlayerMaxHealth = player.maxHealth;
        PlayerHealth = player.currHealth;
        items = playerInventory.MyItems;
        CommonKeys = playerInventory.commonKeys;
        UnCommonKeys = playerInventory.uncommonKeys;
        BossKeys = playerInventory.bossKeys;
        Money = playerInventory.money;
        AmmoLeft = playerInventory.currentAmmo;


        //I may need this method when I start actually saving games
        //Chest[] chestsArray = FindObjectsOfType<Chest>();
        //foreach (Chest currentChest in chestsArray)
        //{
        //    chests[currentChest.name] = currentChest.isOpen;
        //}
    }
}
