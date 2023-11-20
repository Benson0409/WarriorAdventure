using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [Header("事件監聽")]
    public SceneLoadEventSo loadEventSo;
    public GameSceneSo firstLoadScene;

    private GameSceneSo currentScene;
    private GameSceneSo loadScene;
    private Vector3 positionToGo;
    private bool fadeScreen;
    public float fadeDuration;
    void Awake()
    {
        // Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        currentScene = firstLoadScene;
        currentScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }
    void OnEnable()
    {
        loadEventSo.LoadRequestEvent += OnLoadRequestEvent;
    }


    void OnDisable()
    {
        loadEventSo.LoadRequestEvent += OnLoadRequestEvent;
    }
    private void OnLoadRequestEvent(GameSceneSo locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        loadScene = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        if (currentScene != null)
        {
            StartCoroutine(UnloadPreviousScene());
        }
    }

    private IEnumerator UnloadPreviousScene()
    {
        if (fadeScreen)
        {
            //實現漸入漸出
        }
        yield return new WaitForSeconds(fadeDuration);
        yield return currentScene.sceneReference.UnLoadScene();
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        loadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
    }
}
