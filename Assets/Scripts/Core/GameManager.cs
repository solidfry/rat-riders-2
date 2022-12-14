using System.Collections;
using UnityEngine;
using Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public string deathSceneName;
    public float deathSceneWaitTime = 2f;
    public string nextLevel;
    
    private void OnEnable()
    {
        GameEvents.onPlayerDiedEvent += LoadEndScreen;
        GameEvents.onLoadNextLevelEvent += LoadNextLevel;
    }

    private void OnDisable()
    {
        GameEvents.onPlayerDiedEvent -= LoadEndScreen;
        GameEvents.onLoadNextLevelEvent -= LoadNextLevel;
    }

    private void LoadEndScreen() => DelaySceneLoad(deathSceneName, deathSceneWaitTime);

    void DelaySceneLoad(string sceneToLoad, float delay)
    {
        StartCoroutine(DelaySceneLoadSeconds(sceneToLoad, delay));
    }
        
    IEnumerator DelaySceneLoadSeconds(string sceneToLoad, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }
    
    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
    
    
}
