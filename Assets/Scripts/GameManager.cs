using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (transform.parent != null)
            transform.SetParent(null);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += HandleSceneChange;

        SceneManager.LoadScene("MainMenu");
    }

    private void HandleSceneChange(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("Game"))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        else if (scene.name.Equals("MainMenu"))
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
