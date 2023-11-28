using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : Interactable
{

    public GameObject dialoguePanel;
    public Image npcImage; // Gambar NPC
    public TextMeshProUGUI nameText; // Teks nama NPC
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;

    public GameObject contButton;

    public float wordspeed;
    public bool playerIsClose;

    public string npcName;
    public Sprite npcSprite;

    private bool conversationStarted = false;


    protected override void Start()
    {
        base.Start();
        // Inisialisasi elemen-elemen pada objek UI
        npcImage = dialoguePanel.transform.Find("NPCImage").GetComponent<Image>();
        nameText = dialoguePanel.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        dialogueText = dialoguePanel.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        //contButton = dialoguePanel.transform.Find("ContinueButton").GetComponent<Button>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space) && playerIsClose)
        {
            if (Player.instance != null)
            {
                PlayerMovement playerMovement = Player.instance.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    if (!conversationStarted)
                    {
                        // Spasi pertama untuk membuka percakapan
                        conversationStarted = true;
                        playerMovement.SetInteracting(true);
                        if (dialoguePanel.activeInHierarchy)
                        {
                            zeroText();
                        }
                        else
                        {
                            dialoguePanel.SetActive(true);
                            StartCoroutine(Typing());
                        }
                    }
                    else
                    {
                        // Spasi kedua untuk melanjutkan percakapan
                        NextLine();
                    }
                }
            }
        }

        if (dialogueText.text == dialogue[index])
        {
            contButton.SetActive(true);
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        nameText.text = npcName;
        npcImage.sprite = npcSprite;

        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordspeed);
        }
    }

    public void NextLine()
    {
        contButton.SetActive(false);

        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
            conversationStarted = false; // Reset variabel setelah percakapan selesai

            if (Player.instance != null)
            {
                PlayerMovement playerMovement = Player.instance.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.SetInteracting(false);
                }
            }
        }
    }


    override protected void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            base.OnTriggerEnter2D(other);
            playerIsClose = true;
            
        }
    }

    override protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            base.OnTriggerExit2D(other);
            playerIsClose = false;
            zeroText();
        }
    }
}
