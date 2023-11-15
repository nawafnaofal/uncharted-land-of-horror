using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// Enum yang mendefinisikan berbagai tipe pintu
public enum DoorType
{
    None,
    Key,
    UncommonKey,
    BossKey,
    KillEnemies,
    Switch
}

// Kelas yang mengatur perilaku pintu dan mengimplementasikan antarmuka Interactable
public class Door : Interactable
{
    // Tipe pintu yang diperlukan untuk membukanya
    public DoorType OpenBy;
    // Sprite pintu saat terbuka
    public Sprite openDoorSprite;
    // Sprite pintu saat tertutup
    Sprite closedDoorSprite;
    // Komponen untuk merender sprite pintu
    SpriteRenderer spriteRenderer;

    // Durasi dan kekuatan getaran saat pintu diguncang
    float shakeDuration = .2f, shakeStrength = .2f;
    // Jumlah getaran
    int vibration = 50;
    // Jumlah musuh yang harus dikalahkan sebelum membuka pintu
    int enemiesLeft = 0;

    // Komponen RoomMove untuk mengatur perpindahan pemain antar ruangan
    RoomMove rm;

    // ID unik untuk pintu berdasarkan nama, posisi, dan adegan saat ini
    string uniqueID;

    // Metode Start() yang dijalankan saat objek pintu diinisialisasi
    protected override void Start()
    {
        base.Start();

        // Mengambil komponen SpriteRenderer dari pintu
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Menyimpan sprite pintu tertutup sebagai referensi
        closedDoorSprite = spriteRenderer.sprite;
        // Mengambil komponen RoomMove yang terpasang pada pintu
        rm = GetComponent<RoomMove>();

        // Membentuk ID unik untuk pintu
        uniqueID = UnityEngine.SceneManagement.SceneManager.GetActiveScene() + name + transform.position;

        // Mengecek apakah InfoManager memiliki data pintu yang sesuai dengan uniqueID
        if (InfoManager.Instance.doors.TryGetValue(uniqueID, out DoorType temp))
        {
            OpenBy = temp;

            // Jika pintu terbuka, maka set tipe pintu menjadi None
            if (OpenBy == DoorType.None)
            {
                SetDoorType(DoorType.None);
            }
        }
        else
        {
            // Jika tidak ada data pintu yang sesuai dengan uniqueID, tambahkan pintu baru dengan tipe OpenBy ke InfoManager
            InfoManager.Instance.doors.Add(uniqueID, OpenBy);
        }
    }

    // Metode yang dijalankan saat pemain berinteraksi dengan pintu
    protected override void Interacting()
    {
        // Mengambil komponen Inventory dari pemain
        Inventory pI = player.GetComponent<Inventory>();

        // Memeriksa tipe pintu dan mengambil tindakan yang sesuai berdasarkan tipe pintu
        switch (OpenBy)
        {
            case DoorType.None:
                StartCoroutine(OpenDoor());
                break;
            case DoorType.Key:
                if (pI.commonKeys > 0)
                {
                    pI.RemoveItem(ItemType.CommonKey);
                    StartCoroutine(OpenDoor());
                }
                else
                {
                    ShakeDoor();
                }
                break;
            case DoorType.UncommonKey:
                if (pI.uncommonKeys > 0)
                {
                    pI.RemoveItem(ItemType.UncommonKey);
                    StartCoroutine(OpenDoor());
                }
                else
                {
                    ShakeDoor();
                }
                break;
            case DoorType.BossKey:
                if (pI.bossKeys > 0)
                {
                    pI.RemoveItem(ItemType.BossKey);
                    StartCoroutine(OpenDoor());
                }
                else
                {
                    ShakeDoor();
                }
                break;
            case DoorType.KillEnemies:
                if (enemiesLeft <= 0)
                {
                    StartCoroutine(OpenDoor());
                }
                else
                {
                    ShakeDoor();
                }
                break;
            case DoorType.Switch:
                ShakeDoor();
                break;
            default:
                break;
        }
    }

    // Metode untuk menggoyangkan pintu
    void ShakeDoor()
    {
        transform.DOShakePosition(shakeDuration, shakeStrength, vibration, 90);
    }

    // Metode untuk membuka pintu
    IEnumerator OpenDoor()
    {
        // Mengubah tipe pintu menjadi None
        SetDoorType(DoorType.None);

        // Menghapus data pintu unik
        InfoManager.Instance.doors[uniqueID] = DoorType.None;

        // Menonaktifkan komponen PlayerMovement pada pemain
        player.GetComponent<PlayerMovement>().enabled = false;

        // Mengganti sprite pintu menjadi sprite pintu terbuka
        spriteRenderer.sprite = openDoorSprite;

        // Menunggu selama 0.5 detik
        yield return new WaitForSeconds(.5f);

        // Memindahkan pemain ke lokasi yang ditentukan oleh RoomMove
        rm.MovePlayer(player.GetComponent<Transform>());

        // Mengganti sprite pintu menjadi sprite pintu tertutup
        spriteRenderer.sprite = closedDoorSprite;

        // Menunggu selama 0.2 detik
        yield return new WaitForSeconds(.2f);

        // Menghancurkan komponen RoomMove yang tidak diperlukan lagi
        Destroy(rm);

        // Mengaktifkan kembali komponen PlayerMovement pada pemain
        player.GetComponent<PlayerMovement>().enabled = true;
    }

    // Metode untuk mengatur tipe pintu
    public void SetDoorType(DoorType newOpenBy)
    {
        if (OpenBy != newOpenBy)
            OpenBy = newOpenBy;
    }
}
