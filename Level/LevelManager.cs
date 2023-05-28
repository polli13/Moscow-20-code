using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private int loadedScene;

    public void LoadLevel()
    {
        SceneManager.LoadSceneAsync(loadedScene);
    }
    public void LoadLevel(int _sceneIndex)
    {
        SceneManager.LoadSceneAsync(_sceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
