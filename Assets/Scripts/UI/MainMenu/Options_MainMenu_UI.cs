using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Options_MainMenu_UI : MonoBehaviour
{
    public void SaveAndReturnToTitle()
    {
        StartCoroutine(SaveAndReturnToTitle_Coroutine());
    }

    private IEnumerator SaveAndReturnToTitle_Coroutine()
    {
        SaveManager.instance.SaveGame();

        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene("MainMenu");
    }

}
