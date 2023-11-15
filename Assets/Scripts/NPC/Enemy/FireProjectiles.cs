using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectiles : MonoBehaviour
{
    public GameObject projectile;
    [HideInInspector]
    float shootDelayTimer;
    public Vector2 shootDelayRange = new Vector2(.5f, 2);

    [HideInInspector]
    public bool firing;
    
    GameObject target;

    // Player Parameters
    bool onPlayer;
    Animator anim;
    Inventory playerInventory;

    private void Start()
    {
        Player player = GetComponent<Player>();
        if(player != null)
        {
            onPlayer = true;
            anim = GetComponent<Animator>();
            playerInventory = GetComponent<Inventory>();
        }
        else
        {
            target = FindObjectOfType<Player>().gameObject;
            shootDelayTimer = Random.Range(shootDelayRange.x, shootDelayRange.y);
        }
    }

    private void Update()
    {
        shootDelayTimer -= Time.deltaTime;

        if (!onPlayer)
        {
            if (firing)
            {
                if (shootDelayTimer <= 0)
                {
                    Vector2 temp = target.transform.position - transform.position;
                    GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
                    proj.transform.right = temp;
                    shootDelayTimer = Random.Range(shootDelayRange.x, shootDelayRange.y);
                }
            }
        }
        else
        {
            if (shootDelayTimer <= 0)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if(playerInventory.currentAmmo > 0)
                    {
                        GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
                        proj.transform.right = new Vector3(anim.GetFloat("moveX"), anim.GetFloat("moveY"));
                        shootDelayTimer = shootDelayRange.x;
                        playerInventory.RemoveItem(ItemType.Arrow);
                    }
                }
            }
        }
    }
}
