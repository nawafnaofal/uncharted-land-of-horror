using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    void Start()
    {
        // Mengambil referensi Transform dari objek dengan tag "Player" dan menyimpannya di variabel "target"
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;

        // Mendapatkan komponen CinemachineVirtualCamera dari objek ini dan mengatur target kameranya
        GetComponent<CinemachineVirtualCamera>().Follow = target;

        // Mendapatkan komponen CinemachineConfiner dari objek ini dan mengatur pembatasan ruang gerak kamera dengan menggunakan collider PolygonCollider2D pada objek yang sama
        GetComponent<CinemachineConfiner>().m_BoundingShape2D = GetComponentInParent<PolygonCollider2D>();
    }
}
