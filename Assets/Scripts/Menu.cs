using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TMP_InputField playerName;

    public void StartButton()
    {
        if (!string.IsNullOrWhiteSpace(playerName.text))
        {
            Manager.Instance.playerName = playerName.text;
            SceneManager.LoadScene(1);
        }             
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
