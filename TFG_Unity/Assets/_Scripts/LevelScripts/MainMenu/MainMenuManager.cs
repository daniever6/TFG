using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;

    private void Start()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }


    /// <summary>
    /// Empieza una nueva partida
    /// </summary>
    public void StartNewGame()
    {
        SceneManager.LoadScene("Intro");
    }

    /// <summary>
    /// Carga el juego en la ultima escena guardada
    /// </summary>
    public void LoadGame()
    {

    }

    /// <summary>
    /// Abre el panel de opciones
    /// </summary>
    public void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    /// <summary>
    /// Cierra el panel de opciones
    /// </summary>
    public void CloseOptionsPanel()
    {
        optionsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    /// <summary>
    /// Sale de la aplicacion
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
