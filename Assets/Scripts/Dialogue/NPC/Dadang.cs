using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dadang : MonoBehaviour
{
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color firstColor = Color.red;
    [SerializeField] private Color secondColor = Color.green;
    [SerializeField] private Color thirdColor = Color.blue;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        string playerName = ((Ink.Runtime.StringValue) DialogueManager
            .GetInstance()
            .GetVariableState("player_name")).value;

        switch (playerName)
        {
            case "":
                spriteRenderer.color = defaultColor;
                break;
            case "Kirana":
                spriteRenderer.color = firstColor;
                break;
            case "...":
                spriteRenderer.color = secondColor;
                break;
            case "Nengsih":
                spriteRenderer.color = thirdColor;
                break;
            default:
                Debug.LogWarning("Player Name not handled by switch statement: " +  playerName);
                break;
        }
    }
}
