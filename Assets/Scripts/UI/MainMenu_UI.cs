using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] private FadeScreen_UI fadeScreen;

    [Header("Exit Confirm")]
    [SerializeField] private GameObject exitConfirmWindow;

    private void Start()
    {
        if (!SaveManager.instance.HasSaveData())
        {
            continueButton.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        //SceneManager.LoadScene(sceneName);
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSavedData();
        //SceneManager.LoadScene(sceneName);
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void Exit()
    {
        Debug.Log("Game exited");
        Application.Quit();
    }

    private IEnumerator LoadSceneWithFadeEffect(float _delayTime)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delayTime);

        SceneManager.LoadScene(sceneName);
    }

    public void ShowExitConfirmWindow()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //close all the other UIs
            transform.GetChild(i).gameObject.SetActive(false);
            exitConfirmWindow.SetActive(true);
        }
    }

    public void CloseExitConfirmWindow()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //turn on all the other UIs
            transform.GetChild(i).gameObject.SetActive(true);
            exitConfirmWindow.SetActive(false);
        }
    }
}
