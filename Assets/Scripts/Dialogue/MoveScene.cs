using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("CreditScene", LoadSceneMode.Single);
    }
}
