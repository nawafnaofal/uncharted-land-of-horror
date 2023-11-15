using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    // Referensi ke pintu yang akan dibuka oleh sakelar
    public Door door;
    // Sprite yang akan digunakan saat sakelar ditekan
    public Sprite pressedSwitch;
    // Status sakelar (dalam keadaan ditekan atau tidak)
    bool isPressed = false;
    // Sprite sakelar dalam keadaan tidak ditekan
    Sprite unpressedSwitch;
    // Komponen untuk merender sprite sakelar
    SpriteRenderer spriteRenderer;
    // ID unik untuk sakelar berdasarkan nama, posisi, dan adegan saat ini
    string uniqueID;

    private void Start()
    {
        // Mengambil komponen SpriteRenderer dari sakelar
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Menyimpan sprite sakelar dalam keadaan tidak ditekan sebagai referensi
        unpressedSwitch = spriteRenderer.sprite;

        // Membentuk ID unik untuk sakelar
        uniqueID = UnityEngine.SceneManagement.SceneManager.GetActiveScene() + name + transform.position;

        // Mengecek apakah InfoManager memiliki data sakelar yang sesuai dengan uniqueID
        if (InfoManager.Instance.buttons.TryGetValue(uniqueID, out bool storedPressedBool))
        {
            // Jika ada, mengambil status sakelar dari data tersebut
            isPressed = storedPressedBool;

            // Jika sakelar dalam keadaan ditekan, buka pintu terkait
            if (isPressed)
                UnlockDoor();
        }
        else
        {
            // Jika tidak ada data sakelar yang sesuai dengan uniqueID, tambahkan sakelar baru ke InfoManager dengan status awal tidak ditekan
            InfoManager.Instance.buttons.Add(uniqueID, isPressed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Ketika pemain bersentuhan dengan sakelar, buka pintu terkait
            UnlockDoor();
        }
    }

    void UnlockDoor()
    {
        // Mengatur status sakelar menjadi ditekan
        isPressed = true;
        // Memperbarui data status sakelar di InfoManager
        InfoManager.Instance.buttons[uniqueID] = isPressed;
        // Mengganti sprite sakelar menjadi yang ditekan
        spriteRenderer.sprite = pressedSwitch;
        // Mengatur tipe pintu yang terkait dengan sakelar menjadi None (untuk membukanya)
        door.SetDoorType(DoorType.None);
    }

    void LockDoor()
    {
        // Mengatur status sakelar menjadi tidak ditekan
        isPressed = false;
        // Memperbarui data status sakelar di InfoManager
        InfoManager.Instance.buttons[uniqueID] = isPressed;
        // Mengganti sprite sakelar menjadi yang tidak ditekan
        spriteRenderer.sprite = unpressedSwitch;
        // Mengatur tipe pintu yang terkait dengan sakelar menjadi Switch (untuk menguncinya)
        door.SetDoorType(DoorType.Switch);
    }
}
