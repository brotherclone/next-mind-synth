using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    private string _currentSceneName = string.Empty;
    private List<AsyncOperation> _loadOperations;
    public GameObject[] systemPrefabs;
    private readonly List<GameObject> _instancedSystemPrefabs;
    public GameManager(List<GameObject> instancedSystemPrefabs)
    {
        _instancedSystemPrefabs = instancedSystemPrefabs;
    }
    private GameObject _prefabInstance;

    public void LoadScene(string currentScene)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
        if (asyncOperation == null)
        {
            Debug.LogError("[GameManager] Unable to load " + currentScene);
            return;
        }
        asyncOperation.completed += OnLoadOperationComplete;
        _loadOperations.Add(asyncOperation);
        _currentSceneName = currentScene;
    }

    void OnLoadOperationComplete(AsyncOperation asyncOperation)
    {
        if (_loadOperations.Contains(asyncOperation))
        {
            _loadOperations.Remove(asyncOperation);
        }
        Debug.Log("Scene loaded.");
    }

    void OnUnloadOperationComplete(AsyncOperation asyncOperation)
    {
        Debug.Log("Scene unloaded."); 
    }
    public void UnLoadScene(string currentScene)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(currentScene);
        if (asyncOperation == null)
        {
            Debug.LogError("[GameManager] Unable to load " + currentScene);
            return;
        }
        asyncOperation.completed += OnUnloadOperationComplete;
    }
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        _loadOperations = new List<AsyncOperation>();
        LoadScene("Intro");
        InstantiateSystemPrefabs();
    }

    private void InstantiateSystemPrefabs()
    {
        int i;
        for (i = 0; i < systemPrefabs.Length; ++i)
        {
            _prefabInstance = Instantiate(systemPrefabs[i]);
            _instancedSystemPrefabs.Add(_prefabInstance);
        }
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();

        int i;
        for (i = _instancedSystemPrefabs.Count - 1; i >= 0; --i)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }
}
