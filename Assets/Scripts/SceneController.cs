using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UniRx;

public class SceneController : MonoBehaviour
{
    private GameInputs _gameInputs = default;
    private MapController _mapController = default;
    private bool _isClear = default;

    private const string TITLE_NAME = "Title";
    private const string MAIN_NAME = "Main";

    private void Awake()
    {
        _gameInputs = new();
        _gameInputs.Enable();
        _gameInputs.Player.Submit.performed += OnSubmit;
        _gameInputs.Player.Exit.performed += OnExit;

        SceneManager.sceneLoaded += ReadMainSceneInfo;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        _isClear = false;
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name == TITLE_NAME)
        {
            _isClear = false;
            SceneManager.LoadScene(MAIN_NAME);
        }
        else if (SceneManager.GetActiveScene().name == MAIN_NAME && _isClear)
        {
            SceneManager.LoadScene(TITLE_NAME);
        }
    }

    private void OnExit(InputAction.CallbackContext context)
    {
#if !UNITY_EDITOR
      
        Application.Quit();
#endif
    }

    private void ReadMainSceneInfo(Scene nextScene, LoadSceneMode mode)
    {
        // -------------------------------------------------------
        // クリア時にのみシーン遷移できるよう変更する
        // -------------------------------------------------------
        if (nextScene.name != MAIN_NAME) { return; }

        _mapController = FindObjectOfType<MapController>();
        _mapController.OnClear.Subscribe(_ => _isClear = true);
    }

    private void OnDestroy()
    {
        _gameInputs.Dispose();
    }
}
