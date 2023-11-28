using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Analytics;


public class InfoManager : MonoBehaviour
{
    static InfoManager instance;
    public static InfoManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Coba cari instance yang sudah ada di scene
                instance = FindObjectOfType<InfoManager>();

                // Jika tidak ditemukan, buat instance baru
                if (    instance == null)
                {
                    GameObject obj = new GameObject("InfoManager");
                    instance = obj.AddComponent<InfoManager>();
                }

                DontDestroyOnLoad(instance.gameObject);
            }

            //// Debug statement
            //if (instance== null)
            //{
            //    Debug.LogError("InfoManager instance is still null!");
            //}
            //else
            //{
            //    Debug.Log("InfoManager instance found or created.");
            //}

            return instance;
        }
    }

    public Vector2 NewPlayerPosition { get; set; }
    public float PlayerMaxHealth { get; set; }
    public float PlayerHealth { get; set; }
    public float PlayerMagic { get; set; }
    public float Strength { get; set; }
    public string SceneName { get; set; }
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
            Time.timeScale = 1;
            
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

    }

    public void NewGameStats()
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Inventory playerInventory = player.GetComponent<Inventory>();

        // Setel semua atribut player ke nilai awal
        player.maxHealth = 6f;
        player.currHealth = 6f;
        player.strength = 1f;

        // Reset atribut lain di InfoManager ke nilai awal
        PlayerMaxHealth = player.maxHealth;
        PlayerHealth = player.currHealth;

        // Reset atribut inventori ke nilai awal
        playerInventory.MyItems.Clear();  // Mengosongkan item
        playerInventory.commonKeys = 0;
        playerInventory.uncommonKeys = 0;
        playerInventory.bossKeys = 0;
        playerInventory.money = 0;
        playerInventory.currentAmmo = 0;

        // Assign nilai-nilai yang direset ke atribut InfoManager
        items = playerInventory.MyItems;
        CommonKeys = playerInventory.commonKeys;
        UnCommonKeys = playerInventory.uncommonKeys;
        BossKeys = playerInventory.bossKeys;
        Money = playerInventory.money;
        AmmoLeft = playerInventory.currentAmmo;


    }

    public void LoadStats()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            PlayerMaxHealth = data.maxHealth;
            PlayerHealth = data.currHealth;
            Strength = data.strength;
            AmmoLeft = data.currentAmmo;
            CommonKeys = data.commonKeys;
            UnCommonKeys = data.uncommonKeys;
            BossKeys = data.bossKeys;
            Money = data.money;

            // Setel posisi player
            Vector3 position = new Vector3(data.position[0], data.position[1]);
            // Anda perlu mencari objek Player di dalam scene dan mengatur posisinya
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (player != null)
            {
                player.transform.position = position;
            }
            else
            {
                Debug.LogError("Player object not found. Make sure the player object has the 'Player' tag.");
            }

            SceneName = data.sceneName;

            if (!string.IsNullOrEmpty(data.sceneName) && data.sceneName != SceneManager.GetActiveScene().name)
            {
                SceneManager.LoadScene(data.sceneName);
                
            }

            //buat object - object seperti chest switch door
            //Chest[] chestsArray = FindObjectsOfType<Chest>();
            //foreach (Chest currentChest in chestsArray)
            //{
            //    chests[currentChest.name] = currentChest.isOpen;
            //}

        }
    }


}
