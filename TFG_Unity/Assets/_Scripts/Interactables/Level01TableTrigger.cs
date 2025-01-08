using UnityEngine;

public class Level01TableTrigger : MonoBehaviour
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
    //public override void TriggerEvent()
    //{
    //    SaveManager.SaveGameData(_player);
    //    SceneManager.LoadScene(levelName);
    //}
}



