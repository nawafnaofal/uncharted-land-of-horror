using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomMove : MonoBehaviour
{
    // Posisi pergeseran pemain saat berpindah ruangan
    public Vector2 playerPosShift;

    // Menandakan apakah teks nama ruangan diperlukan
    public bool needText;
    // Nama ruangan
    public string roomName;
    // Komponen teks UI yang menampilkan nama ruangan
    TextMeshProUGUI placeText;

    private void Start()
    {
        // Mengambil komponen TextMeshProUGUI dengan nama "RoomTitle" dari objek dalam hierarki
        placeText = GameObject.Find("RoomTitle").GetComponent<TextMeshProUGUI>();
    }

    public void MovePlayer(Transform player)
    {
        if (player.CompareTag("Player"))
        {
            // Memindahkan posisi pemain dengan menggeser sesuai dengan playerPosShift
            player.position += (Vector3)playerPosShift;

            if (needText)
            {
                StartCoroutine(PlaceNameCo());
            }
        }
    }

    IEnumerator PlaceNameCo()
    {
        // Menampilkan teks nama ruangan
        placeText.text = roomName;
        placeText.enabled = true;
        yield return new WaitForSeconds(4f);  // Menunggu selama 4 detik
        placeText.enabled = false;  // Menonaktifkan tampilan teks
    }
}
