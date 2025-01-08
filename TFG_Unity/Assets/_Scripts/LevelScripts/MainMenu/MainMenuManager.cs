using _Scripts.LevelScripts.SaveManager;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        var savePath = Application.persistentDataPath + "/gameState.save";
        var clothPath = Application.persistentDataPath + "/clothingIndex.json";

        if (File.Exists(savePath))
            File.Delete(savePath);
        if (File.Exists(clothPath))
            File.Delete(clothPath);

        SceneManager.LoadScene("Intro");
    }

    /// <summary>
    /// Carga el juego en la ultima escena guardada
    /// </summary>
    public void LoadGame()
    {
        SaveData saveData = SaveManager.LoadGameData();

        if(saveData == null)
        {
            SceneManager.LoadScene("Intro");
            return;
        }

        if(saveData.levelState <= _Scripts.Utilities.LevelState.NivelRecibidor)
        {
            SceneManager.LoadScene("Level_00");
        }
        else
        {
            SceneManager.LoadScene("EscenaMainLevel_Gonzalo");
        }
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
