using _Scripts.Managers;
using _Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : GameplayMonoBehaviour<TutorialManager>
{
    [SerializeField] private Canvas tutorialCanvas;
    [SerializeField] private List<GameObject> tutorialPanels = new();

    private int currentPanelIdx = 0;
    private bool isTutorialActivated = false;

    private void OnEnable()
    {
        DialogueManager.OnDialogueFinish += StartTutorial;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueFinish -= StartTutorial;
    }

    private void Start()
    {
        tutorialCanvas.enabled = false;
        isTutorialActivated = false;
    }

    /// <summary>
    /// Desactiva todos los paneles del canvas del tutorial
    /// </summary>
    private void HideAllPanels()
    {
        foreach (var panel in tutorialPanels)
        {
            panel.SetActive(false);
        }
    }

    /// <summary>
    /// Comienza el tutorial del nivel
    /// </summary>
    public void StartTutorial()
    {
        // Desactiva que se vuelva abrir el panel al acabar un dialogo
        DialogueManager.OnDialogueFinish -= StartTutorial; 

        HideAllPanels();

        isTutorialActivated = true;
        tutorialCanvas.enabled = true;

        currentPanelIdx = 0;

        // Activa el primer panel del tutorial
        tutorialPanels[currentPanelIdx].SetActive(true);
    }

    /// <summary>
    /// Oculta el canvas del tutorial
    /// </summary>
    public void EndTutorial()
    {
        isTutorialActivated = false;
        tutorialCanvas.enabled = false;
    }

    /// <summary>
    /// Activa el siguiente panel del tutorial
    /// </summary>
    public void ShowNextPanel()
    {
        tutorialPanels[currentPanelIdx].SetActive(false); //Desactiva el panel actual

        currentPanelIdx++;
        if(currentPanelIdx >= tutorialPanels.Count)
        {
            EndTutorial();
        }
        else
        {
            tutorialPanels[currentPanelIdx].SetActive(true);
        }
    }

    /// <summary>
    /// Activa el anterior panel del tutorial
    /// </summary>
    public void ShowPreviousPanel()
    {
        tutorialPanels[currentPanelIdx].SetActive(false); //Desactiva el panel actual

        currentPanelIdx--;

        if(currentPanelIdx < 0)
        {
            currentPanelIdx = 0;
        }

        tutorialPanels[currentPanelIdx].SetActive(true);
    }

    /// <summary>
    /// Desactiva el panel del tutorial al pausar el juego
    /// </summary>
    protected override void OnPostPaused()
    {
        base.OnPostPaused();

        tutorialCanvas.enabled = false;
    }

    /// <summary>
    /// Activa el canvas del tutorial despues de reanudar el juego
    /// </summary>
    protected override void OnPostResumed()
    {
        base.OnPostResumed();

        if (isTutorialActivated)
        {
            tutorialCanvas.enabled = true;
        }
    }
}
