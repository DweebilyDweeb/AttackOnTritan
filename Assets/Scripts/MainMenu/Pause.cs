using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {
    public Canvas MainCanvas;
    public Canvas OptionsCanvas;
    public Canvas PauseCanvas;
    private float savedTimeScale;

    void Awake()
    {
        if (PauseCanvas == null && OptionsCanvas == null)
            return;
        if (OptionsCanvas != null)
            OptionsCanvas.enabled = false;
        if (PauseCanvas != null)
            PauseCanvas.enabled = false;

    }
    void Start()
    {
        savedTimeScale = 1.0f;
        UnpauseTime();
    }

    public void MainOn()
    {
        UnpauseTime();
        MainCanvas.enabled = true;
        PauseCanvas.enabled = false;
        OptionsCanvas.enabled = false;
    }
    public void PauseOn()
    {
        PauseTime();
        MainCanvas.enabled = false;
        PauseCanvas.enabled = true;
        OptionsCanvas.enabled = false;
    }
    public void PauseReturn()
    {
        MainCanvas.enabled = false;
        PauseCanvas.enabled = true;
        OptionsCanvas.enabled = false;
    }

    public void OptionsOn()
    {
        MainCanvas.enabled = false;
        PauseCanvas.enabled = false;
        OptionsCanvas.enabled = true;
    }

    public void PauseTime()
    {
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0;
        //AudioListener.pause = true;
    }
    public void UnpauseTime()
    {
        Time.timeScale = savedTimeScale;
        AudioListener.pause = false;
    }


    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
