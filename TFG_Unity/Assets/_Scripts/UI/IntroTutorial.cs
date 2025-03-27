using _Scripts.LevelScripts.SaveManager;
using _Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IntroTutorial : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCanvas;

    public void Start()
    {
        var saveData = SaveManager.LoadGameData();

        if(saveData != null)
        {
            return;
        }

        tutorialCanvas.SetActive(true);
        UserInput.CanMove = false;
    }

    /// <summary>
    /// Cierra el canvas del tutorial
    /// </summary>
    public void CloseCanvas()
    {
        tutorialCanvas.SetActive(false);
        UserInput.CanMove = true;
    }
}
