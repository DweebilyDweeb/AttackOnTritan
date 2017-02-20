using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    public Canvas MainCanvas;
    public Canvas OptionsCanvas;
    public Canvas LevelSelectCanvas;
    public Canvas PauseCanvas;
    private float savedTimeScale;
    void Awake()
    {
        if (OptionsCanvas == null && LevelSelectCanvas == null && PauseCanvas == null)
            return;
        if (OptionsCanvas != null)
        OptionsCanvas.enabled = false;

        if (LevelSelectCanvas != null)
        LevelSelectCanvas.enabled = false;

        if (PauseCanvas != null)
        PauseCanvas.enabled = false;
    }
    void Start()
    {
        AudioListener.pause = false;
        Time.timeScale = 1;
    }


    public void MainOn()
    {
        OptionsCanvas.enabled = false;
        MainCanvas.enabled = true;
        LevelSelectCanvas.enabled = false;
        PauseCanvas.enabled = false;
    }
    public void OptionsOn()
    {
        OptionsCanvas.enabled = true;
        MainCanvas.enabled = false;
        LevelSelectCanvas.enabled = false;
        PauseCanvas.enabled = false;
    }

    public void LevelSelectOn()
    {
        OptionsCanvas.enabled = false;
        MainCanvas.enabled = false;
        LevelSelectCanvas.enabled = true;
        PauseCanvas.enabled = false;
    }

    public void PauseOn()
    {
        OptionsCanvas.enabled = false;
        MainCanvas.enabled = false;
        LevelSelectCanvas.enabled = false;
        PauseCanvas.enabled = true;
    }


    public void GoToShowcase()
    {
        SceneManager.LoadScene("Showcase");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
