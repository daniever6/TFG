using _Scripts.Dialogues;
using _Scripts.Managers;
using Cinemachine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : _Scripts.Utilities.Singleton<IntroManager>
{
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera startCamera;
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera alumno1Camera;
    [SerializeField] private CinemachineVirtualCamera alumno2Camera;
    [SerializeField] private CinemachineVirtualCamera alumno3Camera;

    [Header ("Animators")]
    [SerializeField] private Animator teacherAnimator;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator alumno1Animator;
    [SerializeField] private Animator alumno2Animator;
    [SerializeField] private Animator alumno3Animator;

    [Header("Otros")]
    [SerializeField] private Image fadePanel;

    private string currentPlayerTalking = "";
    private float animationDuration = 2f;

    private void OnEnable()
    {
        DialogueManager.OnCharacterChanged += ChangePlayerCamera;
        DialogueManager.OnDialogueFinish += LoadNextLevel;
    }

    private void OnDisable()
    {
        DialogueManager.OnCharacterChanged -= ChangePlayerCamera;
        DialogueManager.OnDialogueFinish -= LoadNextLevel;
    }

    private void Awake()
    {
        base.Awake();
        startCamera.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        startCamera.enabled = false;
        DeserializeDialogues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Metodo que activa y desactiva las camaras de la cinematica 
    /// en funcion del personaje que esta hablando
    /// </summary>
    /// <param name="character">Personaje actual que habla</param>
    private void ChangePlayerCamera(string character)
    {
        //Cambia la animacion del personaje que deja de hablar
        GetCharacterAnimator(currentPlayerTalking)?.CrossFade("Idle", 0.2f);

        currentPlayerTalking = character;

        // Cambia la animacion del personaje que comienza a hablar
        GetCharacterAnimator(character)?.CrossFade("Talk", 0.2f);

        switch (character)
        {
            case "Profesor":
                alumno1Camera.enabled = false;
                alumno2Camera.enabled = false;
                alumno3Camera.enabled = false;
                playerCamera.enabled = false;

                mainCamera.enabled = true;
                break;

            case "Alumno 1":
                alumno2Camera.enabled = false;
                alumno3Camera.enabled = false;
                playerCamera.enabled = false;

                alumno1Camera.enabled = true;
                break;

            case "Alumno 2":
                alumno1Camera.enabled = false;
                alumno3Camera.enabled = false;
                playerCamera.enabled = false;

                alumno2Camera.enabled = true;
                break;

            case "Alumno 3":
                alumno2Camera.enabled = false;
                alumno3Camera.enabled = false;
                playerCamera.enabled = false;

                alumno3Camera.enabled = true;
                break;

            case "Tú":
                alumno1Camera.enabled = false;
                alumno2Camera.enabled = false;
                alumno3Camera.enabled = false;

                playerCamera.enabled = true;
                break;

            default:
                alumno1Camera.enabled = false;
                alumno2Camera.enabled = false;
                alumno3Camera.enabled = false;
                playerCamera.enabled = false;

                mainCamera.enabled = true;
                break;
        }
    }

    /// <summary>
    /// Devuelve el animator de personaje indicado
    /// </summary>
    /// <param name="characterName">Nombre del personaje</param>
    /// <returns>Animator del personaje</returns>
    private Animator GetCharacterAnimator(string characterName)
    {
        Animator animator = null;

        switch (characterName)
        {
            case "Profesor":
                return teacherAnimator;

            case "Tú":
                return playerAnimator;

            case "Alumno 1":
                return alumno1Animator;

            case "Alumno 2":
                return alumno2Animator;

            case "Alumno 3":
                return alumno3Animator;
        }

        return animator;
    }

    /// <summary>
    /// Deseraliza el JSON de dialogos de la introduccion
    /// </summary>
    private void DeserializeDialogues()
    {
        Dictionary<string, Queue<Dialogue>> _dialoguesDictionary = new();

        TextAsset dialogueJson = Resources.Load<TextAsset>("JSONs/DialogueJSONs");
        if (UnityObjectUtility.IsUnityNull(dialogueJson)) return;

        DialogueStages stages = JsonUtility.FromJson<DialogueStages>(dialogueJson.text);

        foreach (DialogueStage stage in stages.Stages)
        {
            Queue<Dialogue> dialoguesQueue = new Queue<Dialogue>(stage.Dialogues);
            _dialoguesDictionary.Add(stage.Stage, dialoguesQueue);
        }

        DialogueManager.Instance.GetDialogues(_dialoguesDictionary["Tutorial"].ToArray());
    }

    /// <summary>
    /// Carga el siguiente nivel
    /// </summary>
    private void LoadNextLevel()
    {
        fadePanel.gameObject.transform.parent.gameObject.SetActive(true); //Activa el canvas donde se encuentra el panel
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// Animacion de Fade de un panel a negro
    /// </summary>
    /// <returns></returns>
    private IEnumerator<GameObject> FadeIn()
    {
        
        Color panelColor = fadePanel.color;
        float startAlpha = panelColor.a;

        for (float t = 0; t < animationDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(startAlpha, 1f, t / animationDuration);
            panelColor.a = alpha;
            fadePanel.color = panelColor;
            yield return null;
        }

        panelColor.a = 1f;
        fadePanel.color = panelColor;

        SceneManager.LoadScene("Level_00");
    }
}
