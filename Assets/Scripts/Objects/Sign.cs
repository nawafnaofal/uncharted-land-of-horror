using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Sign : Interactable
{
    public GameObject dialogueBox;       // Referensi ke GameObject dialog box
    //public Text dialogueText;
    TextMeshProUGUI dialogueText;        // Referensi ke komponen TextMeshProUGUI
    public string dialogue;              // Isi teks dialog

    protected override void Start()
    {
        base.Start();
        // Menginisialisasi komponen TextMeshProUGUI dari dialog box
        dialogueText = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
    }

    protected override void Update()
    {
        base.Update();
        if (playerInRange && Input.GetButtonDown("Submit"))
        {
            // Menampilkan atau menyembunyikan dialog box saat tombol "Submit" ditekan
            dialogueBox.SetActive(!dialogueBox.activeSelf);
        }
    }

    override protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            base.OnTriggerEnter2D(other);
            // Menampilkan teks dialog saat pemain mendekati objek interaktif
            dialogueText.text = dialogue;
        }
    }

    override protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            base.OnTriggerExit2D(other);
            // Menyembunyikan dialog box saat pemain menjauhi objek interaktif
            dialogueBox.SetActive(false);
        }
    }
}
