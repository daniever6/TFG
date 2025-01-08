using System;
using System.IO;
using _Scripts.LevelScripts.SaveManager;
using _Scripts.Managers;
using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecibidorDoorTrigger : Trigger
{
    [SerializeField] private string levelName;
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Carga la escena indicada
    /// </summary>
    public override void TriggerEvent()
    {
        var savePath = Application.persistentDataPath + "/gameState.save";

        if (File.Exists(savePath))
            File.Delete(savePath);

        SceneManager.LoadScene(levelName);
        LevelManager.Instance.ChangeLevelState(LevelState.NivelBases);
    }
}
