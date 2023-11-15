using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    // Array untuk menyimpan item yang dapat dikumpulkan
    public GameObject[] collectableItems;
    // Peluang spawn item (dalam persentase)
    public int chanceOfSpawn = 50;

    public GameObject GetRandomItem()
    {
        // Menghasilkan angka acak antara 0 dan 100 untuk membandingkan dengan chanceOfSpawn
        int chance = Random.Range(0, 100);

        // Jika angka acak kurang dari atau sama dengan peluang spawn...
        if (chance <= chanceOfSpawn)
        {
            // ...mengembalikan item yang dipilih secara acak dari collectableItems.
            return collectableItems[Random.Range(0, collectableItems.Length)];
        }

        // Jika angka acak lebih besar dari peluang spawn, maka tidak ada item yang dihasilkan.
        return null;
    }
}
