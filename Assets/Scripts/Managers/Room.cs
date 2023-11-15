using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    List<GameObject> gameObjects = new List<GameObject>();
    Collider2D player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject go in gameObjects)
            {
                go.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.gameObject.activeInHierarchy)
                return;
            foreach (GameObject go in gameObjects)
            {
                go.SetActive(false);
            }
        }
    }

    private void Start()
    {
        Invoke("Init", .2f);
    }


    void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        bool playerPresent = GetComponent<PolygonCollider2D>().IsTouching(player);

        for (int i = 0; i < transform.childCount; i++)
        {
            gameObjects.Add(transform.GetChild(i).gameObject);
            gameObjects[i].SetActive(playerPresent);
        }
    }
}
