using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour
{

    [Header("玩家座標")]
    public Transform playerTrans;
    public Vector3 firstPosition;

    [Header("事件監聽")]
    public SceneLoadEventSo loadEventSo;
    public GameSceneSo firstLoadScene;

    [Header("訊息廣播")]
    public VoidEventSo afterSceneLoadEventSo;

    //讀取場景傳送之訊息
    //當前場景
    private GameSceneSo currentScene;

    //即將加載場景
    private GameSceneSo loadScene;

    //玩家要前往位置
    private Vector3 positionToGo;

    [Header("淡入淡出")]
    public CanvasGroup fadeCanva;

    [Header("變量控制")]
    private bool isLoading;
    private bool fadeScreen;
    public float fadeDuration;
    // void Awake()
    // {
    //     // Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
    //     currentScene = firstLoadScene;
    //     currentScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    // }

    //Menu完成之後要更改
    void Start()
    {
        NewGame();
    }
    void OnEnable()
    {
        loadEventSo.LoadRequestEvent += OnLoadRequestEvent;
    }

    void OnDisable()
    {
        loadEventSo.LoadRequestEvent += OnLoadRequestEvent;
    }

    private void NewGame()
    {
        loadScene = firstLoadScene;
        OnLoadRequestEvent(loadScene, firstPosition, true);
    }

    /// <summary>
    /// 場景加載事件請求
    /// </summary>
    /// <param name="locationToLoad"></param>
    /// <param name="posToGo"></param>
    /// <param name="fadeScreen"></param>
    private void OnLoadRequestEvent(GameSceneSo locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;

        loadScene = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        if (currentScene != null)
        {
            StartCoroutine(UnloadPreviousScene());
        }
        else
        {

            LoadNewScene();
        }
    }

    private IEnumerator UnloadPreviousScene()
    {
        //漸入
        if (fadeScreen)
        {
            StartCoroutine(FadeOutScenes());
        }
        yield return new WaitForSeconds(fadeDuration);
        yield return currentScene.sceneReference.UnLoadScene();

        //關閉人物
        playerTrans.gameObject.SetActive(false);

        //載入新場景
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = loadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadComoleted;
    }

    /// <summary>
    /// 場景加載完成後
    /// </summary>
    /// <param name="obj"></param>
    private void OnLoadComoleted(AsyncOperationHandle<SceneInstance> obj)
    {
        currentScene = loadScene;
        playerTrans.position = positionToGo;

        //開啟人物
        playerTrans.gameObject.SetActive(true);

        //漸出
        if (fadeScreen)
        {
            StartCoroutine(FadeInScenes());
        }
        isLoading = false;

        //場景加載完成後廣播訊息
        afterSceneLoadEventSo.RaiseEvent();
    }

    public IEnumerator FadeOutScenes()
    {
        yield return FadeOut(fadeDuration);
    }
    public IEnumerator FadeInScenes()
    {
        yield return FadeIn(fadeDuration);
    }
    public IEnumerator FadeOut(float time)
    {
        while (fadeCanva.alpha < 1)
        {
            fadeCanva.alpha += Time.deltaTime / time;
            yield return null;
        }
    }
    public IEnumerator FadeIn(float time)
    {
        while (fadeCanva.alpha != 0)
        {
            fadeCanva.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }
}
