using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InitializationScenePass : MonoBehaviour
{
    #region Variables / Properties

    public string NextSceneName;

    #endregion Variables / Properties

    #region Engine Hooks

    public void Start()
    {
        StartCoroutine(WaitForRepositoriesToLoad());
    }

    #endregion Engine Hooks

    #region Methods

    public IEnumerator WaitForRepositoriesToLoad()
    {
        yield return null;
        SceneManager.LoadScene(NextSceneName);
    }

    #endregion Methods
}
