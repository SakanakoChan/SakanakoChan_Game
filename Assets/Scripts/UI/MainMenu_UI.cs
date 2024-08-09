using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        if (!SaveManager.instance.HasSaveData())
        {
            continueButton.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSavedData();
        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
        Debug.Log("Game exited");
    }
}
