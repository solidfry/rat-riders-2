using UnityEngine;
using Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public string deathSceneName;

    private void OnEnable()
    {
        GameEvents.onPlayerDiedEvent += LoadEndScreen;
    }

    private void OnDisable()
    {
        GameEvents.onPlayerDiedEvent -= LoadEndScreen;
    }

    private void LoadEndScreen()
    {
        SceneManager.LoadScene(deathSceneName);
    }
}
